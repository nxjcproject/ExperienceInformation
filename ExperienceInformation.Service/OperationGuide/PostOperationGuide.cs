using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SqlServerDataAdapter;
using ExperienceInformation.Infrastructure.Configuration;

namespace ExperienceInformation.Service.OperationGuide
{
    public class PostOperationGuide
    {
        private static readonly string _connStr = ConnectionStringFactory.NXJCConnectionString;
        private static readonly ISqlServerDataFactory _dataFactory = new SqlServerDataFactory(_connStr);
        public static DataTable GetPostOperationGuideInfo(string myCreateYear, string myKeyword)
        {
            string m_SqlCondition = "";
            string m_Sql = @"Select
                    A.PostOperationKnowledgeId as PostOperationKnowledgeId,  
                    A.Keyword as Keyword, 
                    A.PostName as PostName, 
                    A.OrganizationID as OrganizationId,
                    D.Name as OrganizationName, 
                    A.PostOperationKnowledgeName as PostOperationKnowledgeName,
                    B.TYPE_NAME as PostOperationKnowledgeType,
                    A.PostOperationKnowledgeGroup as PostOperationKnowledgeGroup,
                    A.Propounder as Propounder,
                    A.ProposedTime as ProposedTime,
                    C.USER_NAME as CreateName,
                    A.CreateTime as CreateTime,
                    A.Remarks as Remarks
                    from experience_PostOperationKnowledge A 
                    left join system_TypeDictionary B on A.PostOperationKnowledgeType = B.TYPE_ID 
                    left join IndustryEnergy_SH.dbo.users C on A.Creator = C.USER_ID
                    left join system_Organization D on A.OrganizationID = D.OrganizationID   
                    where A.PostOperationKnowledgeId = A.PostOperationKnowledgeId
                    {0} 
					order by A.OrganizationID, A.CreateTime desc";
            if (myCreateYear != "")
            {
                m_SqlCondition = m_SqlCondition + string.Format(" and CONVERT(varchar(4) , A.CreateTime, 112 ) = '{0}' ", myCreateYear);
            }
            if (myKeyword != "")
            {
                m_SqlCondition = m_SqlCondition + string.Format(" and A.Keyword like '%{0}%' ", myKeyword);
            }
            m_Sql = string.Format(m_Sql, m_SqlCondition);
            try
            {
                DataTable m_Result = _dataFactory.Query(m_Sql);
                return m_Result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static DataTable GetPostOperationGuideInfoById(string myPostOperationKnowledgeId)
        {
            string m_Sql = @"Select
                    A.PostOperationKnowledgeId as PostOperationKnowledgeId,  
                    A.Keyword as Keyword, 
                    A.PostName as PostName, 
                    A.OrganizationID as OrganizationId,
                    A.PostOperationKnowledgeName as PostOperationKnowledgeName,
                    B.TYPE_NAME as PostOperationKnowledgeType,
                    A.PostOperationKnowledgeGroup as PostOperationKnowledgeGroup,
                    A.Propounder as Propounder,
                    A.ProposedTime as ProposedTime,
                    C.USER_NAME as CreateName,
                    A.CreateTime as CreateTime,
                    A.Remarks as Remarks
                    from experience_PostOperationKnowledge A 
                    left join system_TypeDictionary B on A.PostOperationKnowledgeType = B.TYPE_ID 
                    left join IndustryEnergy_SH.dbo.users C on A.Creator = C.USER_ID   
                    where A.PostOperationKnowledgeId = '{0}'
					order by A.CreateTime desc";
            m_Sql = string.Format(m_Sql, myPostOperationKnowledgeId);
            try
            {
                DataTable m_Result = _dataFactory.Query(m_Sql);
                return m_Result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string GetPostOperationGuideTextById(string myPostOperationKnowledgeId)
        {
            string m_Sql = @"Select
                    A.PostOperationKnowledgeText as PostOperationKnowledgeText 
                    from experience_PostOperationKnowledge A 
                    where A.PostOperationKnowledgeId = '{0}'";
            m_Sql = string.Format(m_Sql, myPostOperationKnowledgeId);
            try
            {
                DataTable m_Result = _dataFactory.Query(m_Sql);
                if (m_Result != null && m_Result.Rows.Count > 0)
                {
                    return m_Result.Rows[0]["PostOperationKnowledgeText"].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public static int AddOperationGuide(string myPostOperationKnowledgeId, string myPostOperationKnowledgeName, string myKeyword, string myPostName, string myPostOperationKnowledgeType, string myPostOperationKnowledgeGroup,
                   string myPostOperationKnowledgeText, string myPropounder, string myProposedTime, string myCreator, string myRemarks)
        {
            string m_OrganizationID = ExperienceInformation.Infrastructure.Configuration.WebConfigurations.StationId;
            string m_Sql = @"Insert into experience_PostOperationKnowledge 
                ( PostOperationKnowledgeId, Keyword, OrganizationID, PostName, PostOperationKnowledgeName, PostOperationKnowledgeType, PostOperationKnowledgeGroup,
                   PostOperationKnowledgeText, Propounder, ProposedTime, Creator,CreateTime,Remarks,ModifyFlag) 
                values
                ( @PostOperationKnowledgeId, @Keyword, @OrganizationID, @PostName, @PostOperationKnowledgeName, @PostOperationKnowledgeType, @PostOperationKnowledgeGroup,
                   @PostOperationKnowledgeText, @Propounder, @ProposedTime, @Creator, @CreateTime, @Remarks, @ModifyFlag)";
            SqlParameter[] m_Parameters = { new SqlParameter("@PostOperationKnowledgeId", myPostOperationKnowledgeId),
                                          new SqlParameter("@Keyword", myKeyword),
                                          new SqlParameter("@OrganizationID", m_OrganizationID),
                                          new SqlParameter("@PostName", myPostName),
                                          new SqlParameter("@PostOperationKnowledgeName", myPostOperationKnowledgeName),
                                          new SqlParameter("@PostOperationKnowledgeType", myPostOperationKnowledgeType),
                                          new SqlParameter("@PostOperationKnowledgeGroup", myPostOperationKnowledgeGroup),
                                          new SqlParameter("@PostOperationKnowledgeText", myPostOperationKnowledgeText),
                                          new SqlParameter("@Propounder", myPropounder),
                                          new SqlParameter("@ProposedTime", myProposedTime),
                                          new SqlParameter("@Creator", myCreator),
                                          new SqlParameter("@CreateTime", DateTime.Now),
                                          new SqlParameter("@Remarks", myRemarks),
                                          new SqlParameter("@ModifyFlag", "123")};
            try
            {
                return _dataFactory.ExecuteSQL(m_Sql, m_Parameters);
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public static int ModifyOperationGuideById(string myPostOperationKnowledgeId, string myPostOperationKnowledgeName, string myKeyword, string myOrganizationID, string myPostName, string myPostOperationKnowledgeType, string myPostOperationKnowledgeGroup,
                   string myPostOperationKnowledgeText, string myPropounder, string myProposedTime, string myCreator, string myRemarks)
        {
            string m_Sql = @"UPDATE experience_PostOperationKnowledge SET
                            Keyword = @Keyword, 
                            OrganizationID = @OrganizationID, 
                            PostName = @PostName,
                            PostOperationKnowledgeName = @PostOperationKnowledgeName, 
                            PostOperationKnowledgeType = @PostOperationKnowledgeType, 
                            PostOperationKnowledgeGroup = @PostOperationKnowledgeGroup,
                            PostOperationKnowledgeText = @PostOperationKnowledgeText, 
                            Propounder = @Propounder, 
                            ProposedTime = @ProposedTime, 
                            Creator = @Creator,
                            CreateTime = @CreateTime,
                            Remarks = @Remarks
                            where PostOperationKnowledgeId = @PostOperationKnowledgeId";
            SqlParameter[] m_Parameters = { new SqlParameter("@PostOperationKnowledgeId", myPostOperationKnowledgeId),
                                          new SqlParameter("@Keyword", myKeyword),
                                          new SqlParameter("@OrganizationID", myOrganizationID),
                                          new SqlParameter("@PostName", myPostName),
                                          new SqlParameter("@PostOperationKnowledgeName", myPostOperationKnowledgeName),
                                          new SqlParameter("@PostOperationKnowledgeType", myPostOperationKnowledgeType),
                                          new SqlParameter("@PostOperationKnowledgeGroup", myPostOperationKnowledgeGroup),
                                          new SqlParameter("@PostOperationKnowledgeText", myPostOperationKnowledgeText),
                                          new SqlParameter("@Propounder", myPropounder),
                                          new SqlParameter("@ProposedTime", myProposedTime),
                                          new SqlParameter("@Creator", myCreator),
                                          new SqlParameter("@CreateTime", DateTime.Now),
                                          new SqlParameter("@Remarks", myRemarks)};
            try
            {
                return _dataFactory.ExecuteSQL(m_Sql, m_Parameters);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public static int DeleteOperationGuideById(string myPostOperationKnowledgeId)
        {
            string m_Sql = @"DELETE FROM experience_PostOperationKnowledge where PostOperationKnowledgeId=@PostOperationKnowledgeId";
            SqlParameter[] m_Parameters = { new SqlParameter("@PostOperationKnowledgeId", myPostOperationKnowledgeId) };
            try
            {
                return _dataFactory.ExecuteSQL(m_Sql, m_Parameters);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public static string GetStationId()
        {
            return ExperienceInformation.Infrastructure.Configuration.WebConfigurations.StationId;
        }
    }
}
