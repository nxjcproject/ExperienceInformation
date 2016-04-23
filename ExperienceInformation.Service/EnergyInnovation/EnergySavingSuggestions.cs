using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SqlServerDataAdapter;
using ExperienceInformation.Infrastructure.Configuration;
namespace ExperienceInformation.Service.EnergyInnovation
{
    public class EnergySavingSuggestions
    {
        private static readonly string _connStr = ConnectionStringFactory.NXJCConnectionString;
        private static readonly ISqlServerDataFactory _dataFactory = new SqlServerDataFactory(_connStr);

        public static DataTable GetEnergySavingSuggestionsInfo(string myCreateYear, string myKeyword, string mySuggestionsType)
        {
            string m_SqlCondition = "";
            string m_Sql = @"Select
                    A.SuggestionsId as SuggestionsId,  
                    A.SuggestionsName as SuggestionsName,
                    A.Keyword as Keyword, 
                    A.PostName as PostName, 
                    A.OrganizationID as OrganizationId,
                    B.TYPE_NAME as SuggestionsType,
                    A.SuggestionsGroup as SuggestionsGroup,
                    A.Propounder as Propounder,
                    A.ProposedTime as ProposedTime,
                    C.USER_NAME as CreateName,
                    A.CreateTime as CreateTime,
                    A.Remarks as Remarks
                    from experience_EnergySavingSuggestions A 
                    left join system_TypeDictionary B on A.SuggestionsType = B.TYPE_ID 
                    left join IndustryEnergy_SH.dbo.users C on A.Creator = C.USER_ID   
                    where A.SuggestionsType = '{1}'
                    {0} 
					order by A.CreateTime desc";
            if (myCreateYear != "")
            {
                m_SqlCondition = m_SqlCondition + string.Format(" and CONVERT(varchar(4) , A.CreateTime, 112 ) = '{0}' ", myCreateYear);
            }
            if (myKeyword != "")
            {
                m_SqlCondition = m_SqlCondition + string.Format(" and A.Keyword like '%{0}%' ", myKeyword);
            }
            m_Sql = string.Format(m_Sql, m_SqlCondition, mySuggestionsType);
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
        public static DataTable GetEnergySavingSuggestionsInfoById(string mySuggestionsId)
        {
            string m_Sql = @"Select
                    A.SuggestionsId as SuggestionsId,  
                    A.SuggestionsName as SuggestionsName,
                    A.Keyword as Keyword, 
                    A.PostName as PostName, 
                    A.OrganizationID as OrganizationId,
                    B.TYPE_NAME as SuggestionsType,
                    A.SuggestionsGroup as SuggestionsGroup,
                    A.Propounder as Propounder,
                    A.ProposedTime as ProposedTime,
                    C.USER_NAME as CreateName,
                    A.CreateTime as CreateTime,
                    A.Remarks as Remarks
                    from experience_EnergySavingSuggestions A 
                    left join system_TypeDictionary B on A.SuggestionsType = B.TYPE_ID 
                    left join IndustryEnergy_SH.dbo.users C on A.Creator = C.USER_ID   
                    where A.SuggestionsId = '{0}'
					order by A.CreateTime desc";
            m_Sql = string.Format(m_Sql, mySuggestionsId);
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
        public static string GetEnergySavingSuggestionsTextById(string mySuggestionsId)
        {
            string m_Sql = @"Select
                    A.SuggestionsText as SuggestionsText 
                    from experience_EnergySavingSuggestions A 
                    where A.SuggestionsId = '{0}'";
            m_Sql = string.Format(m_Sql, mySuggestionsId);
            try
            {
                DataTable m_Result = _dataFactory.Query(m_Sql);
                if (m_Result != null && m_Result.Rows.Count > 0)
                {
                    return m_Result.Rows[0]["SuggestionsText"].ToString();
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
        public static int AddEnergySavingSuggestions(string mySuggestionsName, string myKeyword, string myPostName, string myOrganizationID, string mySuggestionsType, string mySuggestionsGroup,
                   string mySuggestionsText, string myPropounder, string myProposedTime, string myCreator, string myRemarks)
        {
            string m_Sql = @"Insert into experience_EnergySavingSuggestions 
                ( Keyword, OrganizationID, PostName, SuggestionsName, SuggestionsType, SuggestionsGroup,
                   SuggestionsText, Propounder, ProposedTime, Creator,CreateTime,Remarks) 
                values
                ( @Keyword, @OrganizationID, @PostName, @SuggestionsName, @SuggestionsType, @SuggestionsGroup,
                   @SuggestionsText, @Propounder, @ProposedTime, @Creator, @CreateTime, @Remarks)";
            SqlParameter[] m_Parameters = { new SqlParameter("@Keyword", myKeyword),
                                          new SqlParameter("@OrganizationID", myOrganizationID),
                                          new SqlParameter("@PostName", myPostName),
                                          new SqlParameter("@SuggestionsName", mySuggestionsName),
                                          new SqlParameter("@SuggestionsType", mySuggestionsType),
                                          new SqlParameter("@SuggestionsGroup", mySuggestionsGroup),
                                          new SqlParameter("@SuggestionsText", mySuggestionsText),
                                          new SqlParameter("@Propounder", myPropounder),
                                          new SqlParameter("@ProposedTime", myProposedTime),
                                          new SqlParameter("@Creator", myCreator),
                                          new SqlParameter("@CreateTime", DateTime.Now),
                                          new SqlParameter("@Remarks", myRemarks)};
            try
            {
                return _dataFactory.ExecuteSQL(m_Sql, m_Parameters);
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public static int ModifyEnergySavingSuggestionsById(string mySuggestionsId, string mySuggestionsName, string myKeyword, string myPostName, string myOrganizationID, string mySuggestionsType, string mySuggestionsGroup,
                   string mySuggestionsText, string myPropounder, string myProposedTime, string myCreator, string myRemarks)
        {
            string m_Sql = @"UPDATE experience_EnergySavingSuggestions SET
                            Keyword = @Keyword, 
                            OrganizationID = @OrganizationID, 
                            PostName = @PostName,
                            SuggestionsName = @SuggestionsName, 
                            SuggestionsType = @SuggestionsType, 
                            SuggestionsGroup = @SuggestionsGroup,
                            SuggestionsText = @SuggestionsText, 
                            Propounder = @Propounder, 
                            ProposedTime = @ProposedTime, 
                            Creator = @Creator,
                            CreateTime = @CreateTime,
                            Remarks = @Remarks
                            where SuggestionsId = @SuggestionsId";
            SqlParameter[] m_Parameters = { new SqlParameter("@SuggestionsId", mySuggestionsId),
                                          new SqlParameter("@Keyword", myKeyword),
                                          new SqlParameter("@OrganizationID", myOrganizationID),
                                          new SqlParameter("@PostName", myPostName),
                                          new SqlParameter("@SuggestionsName", mySuggestionsName),
                                          new SqlParameter("@SuggestionsType", mySuggestionsType),
                                          new SqlParameter("@SuggestionsGroup", mySuggestionsGroup),
                                          new SqlParameter("@SuggestionsText", mySuggestionsText),
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
        public static int DeleteEnergySavingSuggestionsById(string mySuggestionsId)
        {
            string m_Sql = @"DELETE FROM experience_EnergySavingSuggestions where SuggestionsId=@SuggestionsId";
            SqlParameter[] m_Parameters = { new SqlParameter("@SuggestionsId", mySuggestionsId) };
            try
            {
                return _dataFactory.ExecuteSQL(m_Sql, m_Parameters);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
