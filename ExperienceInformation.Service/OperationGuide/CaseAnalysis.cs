using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SqlServerDataAdapter;
using ExperienceInformation.Infrastructure.Configuration;
namespace ExperienceInformation.Service.CaseAnalysis
{
    public class CaseAnalysis
    {
        private static readonly string _connStr = ConnectionStringFactory.NXJCConnectionString;
        private static readonly ISqlServerDataFactory _dataFactory = new SqlServerDataFactory(_connStr);
        public static DataTable GetCaseAnalysisType(string myGroupId)
        {
            string m_Sql = @"Select
                    A.TYPE_ID as TypeId,  
                    A.TYPE_NAME as TypeName  
                    from system_TypeDictionary A  
                    where A.GROUP_ID = '{0}'";
            m_Sql = string.Format(m_Sql, myGroupId);
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
        public static DataTable GetCaseAnalysisInfo(string myCreateYear, string myKeyword, string myCaseAnalysisType, string myCaseAnalysisNature)
        {
            string m_SqlCondition = "";
            string m_Sql = @"Select
                    A.CaseAnalysisId as CaseAnalysisId,  
                    A.CaseAnalysisName as CaseAnalysisName, 
                    A.Keyword as Keyword, 
                    A.CaseAnalysisType as CaseAnalysisType, 
                    B.TYPE_NAME as CaseAnalysisTypeName,
                    A.CaseAnalysisLevel as CaseAnalysisLevel, 
                    A.CaseAnalysisNature as CaseAnalysisNature,
                    A.CaseAnalysisParticipants as CaseAnalysisParticipants,
                    A.CaseAnalysisTime as CaseAnalysisTime,
                    C.USER_NAME as CreateName,
                    A.CreateTime as CreateTime 
                    from experience_CaseAnalysis A 
                    left join system_TypeDictionary B on A.CaseAnalysisType = B.TYPE_ID 
                    left join IndustryEnergy_SH.dbo.users C on A.Creator = C.USER_ID   
                    where A.CaseAnalysisId = A.CaseAnalysisId
                    {0} 
					order by A.CaseAnalysisLevel desc, A.CreateTime desc";
            if (myCreateYear != "")
            {
                m_SqlCondition = m_SqlCondition + string.Format(" and CONVERT(varchar(4) , A.CreateTime, 112 ) = '{0}' ", myCreateYear);
            }
            if (myKeyword != "")
            {
                m_SqlCondition = m_SqlCondition + string.Format(" and A.Keyword like '%{0}%' ", myKeyword);
            }
            if (myCaseAnalysisType != "")
            {
                m_SqlCondition = m_SqlCondition + string.Format(" and A.CaseAnalysisType = '{0}' ", myCaseAnalysisType);
            }
            if (myCaseAnalysisNature != "")
            {
                m_SqlCondition = m_SqlCondition + string.Format(" and A.CaseAnalysisNature = '{0}' ", myCaseAnalysisNature);
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
        public static DataTable GetCaseAnalysisInfoById(string myCaseAnalysisId)
        {
            string m_Sql = @"select
                    A.CaseAnalysisId as CaseAnalysisId,  
                    A.CaseAnalysisName as CaseAnalysisName, 
                    A.Keyword as Keyword, 
                    A.CaseAnalysisType as CaseAnalysisType, 
                    B.TYPE_NAME as CaseAnalysisTypeName,
                    A.CaseAnalysisLevel as CaseAnalysisLevel, 
                    A.CaseAnalysisNature as CaseAnalysisNature,
                    A.CaseAnalysisParticipants as CaseAnalysisParticipants,
                    A.CaseAnalysisTime as CaseAnalysisTime,
                    C.USER_NAME as CreateName,
                    A.CreateTime as CreateTime 
                    from experience_CaseAnalysis A 
                    left join system_TypeDictionary B on A.CaseAnalysisType = B.TYPE_ID 
                    left join IndustryEnergy_SH.dbo.users C on A.Creator = C.USER_ID   
                    where A.CaseAnalysisId =  '{0}'";
            m_Sql = string.Format(m_Sql, myCaseAnalysisId);
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
        public static string GetCaseAnalysisTextById(string myCaseAnalysisId)
        {
            string m_Sql = @"Select
                    A.CaseAnalysisText as CaseAnalysisText 
                    from experience_CaseAnalysis A 
                    where A.CaseAnalysisId = '{0}'";
            m_Sql = string.Format(m_Sql, myCaseAnalysisId);
            try
            {
                DataTable m_Result = _dataFactory.Query(m_Sql);
                if (m_Result != null && m_Result.Rows.Count > 0)
                {
                    return m_Result.Rows[0]["CaseAnalysisText"].ToString();
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
        public static int AddCaseAnalysis(string myCaseAnalysisName, string myKeyword, string myCaseAnalysisType, string myCaseAnalysisLevel,
                   string myCaseAnalysisNature, string myCaseAnalysisText, string myCaseAnalysisParticipants, string myCaseAnalysisTime, string myCreator)
        {
            string m_Sql = @"Insert into experience_CaseAnalysis 
                ( CaseAnalysisName, Keyword, CaseAnalysisType, CaseAnalysisLevel, CaseAnalysisNature, CaseAnalysisText,
                   CaseAnalysisParticipants, CaseAnalysisTime, Creator, CreateTime ) 
                values
                ( @CaseAnalysisName, @Keyword, @CaseAnalysisType, @CaseAnalysisLevel, @CaseAnalysisNature, @CaseAnalysisText,
                   @CaseAnalysisParticipants, @CaseAnalysisTime, @Creator, @CreateTime ) ";
            SqlParameter[] m_Parameters = { new SqlParameter("@CaseAnalysisName", myCaseAnalysisName),
                                          new SqlParameter("@Keyword", myKeyword),
                                          new SqlParameter("@CaseAnalysisType", myCaseAnalysisType),
                                          new SqlParameter("@CaseAnalysisLevel", myCaseAnalysisLevel),
                                          new SqlParameter("@CaseAnalysisNature", myCaseAnalysisNature),
                                          new SqlParameter("@CaseAnalysisText", myCaseAnalysisText),
                                          new SqlParameter("@CaseAnalysisParticipants", myCaseAnalysisParticipants),
                                          new SqlParameter("@CaseAnalysisTime", myCaseAnalysisTime),
                                          new SqlParameter("@Creator", myCreator),
                                          new SqlParameter("@CreateTime", DateTime.Now)};
            try
            {
                return _dataFactory.ExecuteSQL(m_Sql, m_Parameters);
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public static int ModifyCaseAnalysisById(string myCaseAnalysisId, string myCaseAnalysisName, string myKeyword, string myCaseAnalysisType, string myCaseAnalysisLevel,
                   string myCaseAnalysisNature, string myCaseAnalysisText, string myCaseAnalysisParticipants, string myCaseAnalysisTime, string myCreator)
        {
            string m_Sql = @"UPDATE experience_CaseAnalysis SET
                            CaseAnalysisName = @CaseAnalysisName, 
                            Keyword = @Keyword, 
                            CaseAnalysisType = @CaseAnalysisType,
                            CaseAnalysisLevel = @CaseAnalysisLevel, 
                            CaseAnalysisNature = @CaseAnalysisNature, 
                            CaseAnalysisText = @CaseAnalysisText,
                            CaseAnalysisParticipants = @CaseAnalysisParticipants, 
                            CaseAnalysisTime = @CaseAnalysisTime, 
                            Creator = @Creator, 
                            CreateTime = @CreateTime
                            where CaseAnalysisId = @CaseAnalysisId";
            SqlParameter[] m_Parameters = { new SqlParameter("@CaseAnalysisId", myCaseAnalysisId),
                                          new SqlParameter("@CaseAnalysisName", myCaseAnalysisName),
                                          new SqlParameter("@Keyword", myKeyword),
                                          new SqlParameter("@CaseAnalysisType", myCaseAnalysisType),
                                          new SqlParameter("@CaseAnalysisLevel", myCaseAnalysisLevel),
                                          new SqlParameter("@CaseAnalysisNature", myCaseAnalysisNature),
                                          new SqlParameter("@CaseAnalysisText", myCaseAnalysisText),
                                          new SqlParameter("@CaseAnalysisParticipants", myCaseAnalysisParticipants),
                                          new SqlParameter("@CaseAnalysisTime", myCaseAnalysisTime),
                                          new SqlParameter("@Creator", myCreator),
                                          new SqlParameter("@CreateTime", DateTime.Now)};
            try
            {
                return _dataFactory.ExecuteSQL(m_Sql, m_Parameters);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public static int DeleteCaseAnalysisById(string myCaseAnalysisId)
        {
            string m_Sql = @"DELETE FROM experience_CaseAnalysis where CaseAnalysisId=@CaseAnalysisId";
            SqlParameter[] m_Parameters = { new SqlParameter("@CaseAnalysisId", myCaseAnalysisId) };
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
