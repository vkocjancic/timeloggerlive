using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.App.Core.Business;

namespace TimeLogger.App.Core.Repository
{
    public class AssignmentRepository : AssignmentRepositoryBase
    {

        #region Constructors

        public AssignmentRepository(string connectionString) : base(connectionString)
        {
        }

        #endregion

        #region Overridden methods

        public override void ClearUnusedAssignments(Guid userId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(
                    @"delete from ASSIGNMENT
                        where USER_ID = @UserId and not ASSIGNMENT_ID in (
	                        select distinct a.ASSIGNMENT_ID
	                        from ASSIGNMENT a
	                        join TIME_LOG tl on a.ASSIGNMENT_ID = tl.ASSIGNMENT_ID or a.[DESCRIPTION] = tl.[DESCRIPTION])",
                    new { UserId = userId });
            }
        }

        public override void CreateAssignment(Assignment assignment)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(
                    @"insert into ASSIGNMENT ([ASSIGNMENT_ID], [DESCRIPTION], [CREATED], [PROJECT_ID], [USER_ID])
                        values (@Id, @Description, @Created, @ProjectId, @UserID)",
                    new
                    {
                        Id = assignment.Id,
                        Description = assignment.Description,
                        Created = assignment.Created,
                        ProjectId = assignment.ProjectId,
                        UserId = assignment.UserId
                    });
            }
        }

        public override void Delete(Guid id, Guid userId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(
                    @"delete from ASSIGNMENT where USER_ID = @UserId and ASSIGNMENT_ID = @Id",
                    new { UserId = userId, Id = id });
            }
        }

        public override IEnumerable<Assignment> GetAllFor(Guid userId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<Assignment>(
                    @"select a.[ASSIGNMENT_ID] as Id, a.[DESCRIPTION] as Description, a.[CREATED] as Created, a.[PROJECT_ID] as ProjectId, 
                            a.[USER_ID] as UserId, a.[STATUS] as Status, case when a.[FAVOURITE_YN] = 'Y' then 1 else 0 end as IsFavourite, 
                            case when tlc.ASSIGNMENT_ID is null then 0 else tlc.[TIME_LOG_COUNT] end as [TIME_LOG_COUNT]
                        from [ASSIGNMENT] a
                        left join (
	                        select count([TIME_LOG_ID]) as [TIME_LOG_COUNT], [ASSIGNMENT_ID]
	                        from [TIME_LOG]
	                        where [ASSIGNMENT_ID] is not null
	                        group by [ASSIGNMENT_ID]
                        ) tlc on a.[ASSIGNMENT_ID] = tlc.[ASSIGNMENT_ID]
	                    where a.[USER_ID]=@UserId and (a.[STATUS] = 'P' or a.[CREATED] >= @CreatedInLast12Months)",
                    new { UserId = userId, CreatedInLast12Months = DateTime.Today.AddYears(-1).AddDays(1) });
            }
        }

        public override Assignment GetByDescription(string description, Guid userId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<Assignment>(
                    @"select [ASSIGNMENT_ID] as Id, [DESCRIPTION] as Description, [CREATED] as Created, [PROJECT_ID] as ProjectId, [USER_ID] as UserId,
                            [STATUS] as Status, case when [FAVOURITE_YN] = 'Y' then 1 else 0 end as IsFavourite
                        from ASSIGNMENT
	                    where USER_ID=@UserId and [DESCRIPTION]=@Description",
                    new { Description = description, UserId = userId }).FirstOrDefault();
            }
        }

        public override IEnumerable<Assignment> SearchAllFor(string query, Guid userId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<Assignment>(
                    @"select a.[ASSIGNMENT_ID] as Id, a.[DESCRIPTION] as Description, a.[CREATED] as Created, a.[PROJECT_ID] as ProjectId, a.[USER_ID] as UserId,
                        a.[STATUS] as Status, case when a.[FAVOURITE_YN] = 'Y' then 1 else 0 end as IsFavourite
                    from (
	                    select *
	                    from ASSIGNMENT
	                    where USER_ID=@UserId and [DESCRIPTION] like @Query + '%'
	                    union all
	                    select *
	                    from ASSIGNMENT
	                    where USER_ID=@UserId and [DESCRIPTION] like '%' + @Query + '%'
	                    union all
	                    select *
	                    from ASSIGNMENT
	                    where USER_ID=@UserId and [DESCRIPTION] like '%' + @Query
                    ) as a
                    where (a.[STATUS] = 'P' or a.[CREATED] >= @CreatedInLast12Months)
                    group by a.[ASSIGNMENT_ID], a.[DESCRIPTION], a.[CREATED], a.[PROJECT_ID], a.[USER_ID], a.[STATUS], a.[FAVOURITE_YN]
                    order by a.[FAVOURITE_YN] desc, a.[DESCRIPTION], cast(floor(cast(a.[CREATED] as float)) as datetime) desc",
                    new { Query = query, UserId = userId, CreatedInLast12Months = DateTime.Today.AddYears(-1).AddDays(1) }
                    );
            }
        }

        public override void Update(Assignment assignment)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(
                    @"update [ASSIGNMENT] 
                    set [DESCRIPTION]=@Description, [STATUS]=@Status, [FAVOURITE_YN]=@FavouriteYN
                    where [ASSIGNMENT_ID]=@Id",
                    new {
                        Description = assignment.Description,
                        Status = assignment.Status,
                        FavouriteYN = (assignment.IsFavourite) ? "Y" : "N",
                        Id = assignment.Id
                    });
            }
        }


        #endregion

    }
}
