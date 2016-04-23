using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using SqlServerDataAdapter;
using ExperienceInformation.Infrastructure.Configuration;


namespace ExperienceInformation.Service.EnergyProcessNote
{
    public class EnergyProcessRecord
    {
        private static readonly string _connStr = ConnectionStringFactory.NXJCConnectionString;
        private static readonly ISqlServerDataFactory _dataFactory = new SqlServerDataFactory(_connStr);

        public static DataTable GetEnergyProcessRecordInfo(string myStartTime, string myEndTime, string myRecordName, string myDepartmentName, string myRecordType, string myRecordTypeGroup, string myUserId)
        {
            string m_SqlCondition = "";
            string m_Sql = @"Select
                    A.RecordItemId as RecordItemId,  
                    A.RecordName as RecordName,
                    A.Department as DepartmentName, 
                    B.Name as OrganizationName, 
                    C.TYPE_NAME as RecordType,
                    D.USER_NAME as Recorder,
                    A.RecordTime as RecordTime,
                    D.USER_NAME as CreateName,
                    A.CreateTime as CreateTime,
                    A.Remarks as Remarks
                    from experience_EnergyProcessRecord A 
                    left join system_Organization B on A.OrganizationID = B.OrganizationID
                    left join system_TypeDictionary C on A.RecordType = C.TYPE_ID 
                    left join IndustryEnergy_SH.dbo.users D on A.Creator = D.USER_ID   
                    where A.RecordTypeGroup = '{1}' 
                    and A.RecordTime >= '{2}' 
                    and A.RecordTime <= '{3}' 
                    {0} 
					order by A.RecordTime desc, A.CreateTime desc";
            if (myRecordType != "")
            {
                m_SqlCondition = m_SqlCondition + string.Format(" and A.RecordType = '{0}' ", myRecordType);
            }
            if (myRecordName != "")
            {
                m_SqlCondition = m_SqlCondition + string.Format(" and A.RecordName like '%{0}%' ", myRecordName);
            }
            if (myDepartmentName != "")
            {
                m_SqlCondition = m_SqlCondition + string.Format(" and A.Department like '%{0}%' ", myDepartmentName);
            }
            m_Sql = string.Format(m_Sql, m_SqlCondition, myRecordTypeGroup, myStartTime, myEndTime);
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
        public static DataTable GetEnergyProcessRecordInfoById(string myRecordItemId)
        {
            string m_Sql = @"Select
                    A.RecordItemId as RecordItemId,  
                    A.RecordName as RecordName,
                    A.Department as DepartmentName, 
                    A.OrganizationID as OrganizationId, 
                    A.RecordType as RecordType, 
                    A.RecordTypeGroup as RecordTypeGroup,
                    A.Recorder as Recorder,
                    A.RecordTime as RecordTime,
                    A.Creator as Creator,
                    A.CreateTime as CreateTime,
                    A.Remarks as Remarks
                    from experience_EnergyProcessRecord A 
                    where A.RecordItemId = '{0}'";
            m_Sql = string.Format(m_Sql, myRecordItemId);
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
        public static string GetEnergyProcessRecordTextById(string myRecordItemId)
        {
            string m_Sql = @"Select
                    A.RecordText as RecordText 
                    from experience_EnergyProcessRecord A 
                    where A.RecordItemId = '{0}'";
            m_Sql = string.Format(m_Sql, myRecordItemId);
            try
            {
                DataTable m_Result = _dataFactory.Query(m_Sql);
                if (m_Result != null && m_Result.Rows.Count > 0)
                {
                    return m_Result.Rows[0]["RecordText"].ToString();
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
        public static int AddEnergyProcessRecord(string myRecordItemId, string myRecordName, string myDepartmentName, string myOrganizationID, string myRecordType, string myRecordTypeGroup, 
            string myRecorder, string myRecordTime, string myRecordText, string myCreator, string myRemarks)
        {
            string m_Sql = @"Insert into experience_EnergyProcessRecord 
                ( RecordItemId, RecordName, OrganizationID, Department, RecordType, RecordTypeGroup, Recorder, RecordTime,
                   RecordText, Creator, CreateTime, Remarks) 
                values
                ( @RecordItemId, @RecordName, @OrganizationID, @Department, @RecordType, @RecordTypeGroup, @Recorder, @RecordTime,
                   @RecordText, @Creator, @CreateTime, @Remarks)";
            SqlParameter[] m_Parameters = { new SqlParameter("@RecordItemId", myRecordItemId),
                                          new SqlParameter("@RecordName", myRecordName),
                                          new SqlParameter("@OrganizationID", myOrganizationID),
                                          new SqlParameter("@Department", myDepartmentName),
                                          new SqlParameter("@RecordType", myRecordType),
                                          new SqlParameter("@RecordTypeGroup", myRecordTypeGroup),
                                          new SqlParameter("@Recorder", myRecorder),
                                          new SqlParameter("@RecordTime", myRecordTime),
                                          new SqlParameter("@RecordText", myRecordText),
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
        public static int ModifyEnergyProcessRecordById(string myRecordItemId, string myRecordName, string myDepartmentName, string myOrganizationID, string myRecordType, string myRecordTypeGroup,
            string myRecorder, string myRecordTime, string myRecordText, string myCreator, string myRemarks)
        {
            string m_Sql = @"UPDATE experience_EnergyProcessRecord SET
                            RecordName = @RecordName, 
                            OrganizationID = @OrganizationID, 
                            Department = @Department,
                            RecordType = @RecordType,
                            RecordTypeGroup = @RecordTypeGroup, 
                            Recorder = @Recorder, 
                            RecordTime = @RecordTime,
                            RecordText = @RecordText, 
                            Creator = @Creator,
                            CreateTime = @CreateTime,
                            Remarks = @Remarks
                            where RecordItemId = @RecordItemId";
            SqlParameter[] m_Parameters = { new SqlParameter("@RecordItemId", myRecordItemId),
                                          new SqlParameter("@RecordName", myRecordName),
                                          new SqlParameter("@OrganizationID", myOrganizationID),
                                          new SqlParameter("@Department", myDepartmentName),
                                          new SqlParameter("@RecordType", myRecordType),
                                          new SqlParameter("@RecordTypeGroup", myRecordTypeGroup),
                                          new SqlParameter("@Recorder", myRecorder),
                                          new SqlParameter("@RecordTime", myRecordTime),
                                          new SqlParameter("@RecordText", myRecordText),
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
        public static int DeleteEnergyProcessRecordById(string myRecordItemId)
        {
            string m_Sql = @"DELETE FROM experience_EnergyProcessRecord where RecordItemId=@RecordItemId";
            SqlParameter[] m_Parameters = { new SqlParameter("@RecordItemId", myRecordItemId) };
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
