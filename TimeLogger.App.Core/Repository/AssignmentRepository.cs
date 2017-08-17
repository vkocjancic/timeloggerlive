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

        public override IEnumerable<Assignment> GetAllFor(Guid userId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<Assignment>(
                    @"select a.[ASSIGNMENT_ID] as Id, a.[DESCRIPTION] as Description, a.[CREATED] as Created, a.[PROJECT_ID] as ProjectId, a.[USER_ID] as UserId, case when tlc.ASSIGNMENT_ID is null then 0 else tlc.[TIME_LOG_COUNT] end as [TIME_LOG_COUNT]
                        from [ASSIGNMENT] a
                        left join (
	                        select count([TIME_LOG_ID]) as [TIME_LOG_COUNT], [ASSIGNMENT_ID]
	                        from [TIME_LOG]
	                        where [ASSIGNMENT_ID] is not null
	                        group by [ASSIGNMENT_ID]
                        ) tlc on a.[ASSIGNMENT_ID] = tlc.[ASSIGNMENT_ID]
	                    where a.USER_ID=@UserId",
                    new { UserId = userId });
            }
        }

        public override Assignment GetByDescription(string description, Guid userId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<Assignment>(
                    @"select [ASSIGNMENT_ID] as Id, [DESCRIPTION] as Description, [CREATED] as Created, [PROJECT_ID] as ProjectId, [USER_ID] as UserId
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
                    @"select a.[ASSIGNMENT_ID] as Id, a.[DESCRIPTION] as Description, a.[CREATED] as Created, a.[PROJECT_ID] as ProjectId, a.[USER_ID] as UserId
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
                    group by a.[ASSIGNMENT_ID], a.[DESCRIPTION], a.[CREATED], a.[PROJECT_ID], a.[USER_ID]
                    order by a.[DESCRIPTION], cast(floor(cast(a.[CREATED] as float)) as datetime) desc",
                    new { Query = query, UserId = userId }
                    );
            }
        }


        #endregion

    }
}
