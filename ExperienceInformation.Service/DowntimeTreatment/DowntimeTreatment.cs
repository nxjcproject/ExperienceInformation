using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SqlServerDataAdapter;
using ExperienceInformation.Infrastructure.Configuration;

namespace ExperienceInformation.Service.DowntimeTreatment
{
    public class DowntimeTreatment
    {
        private static readonly string _connStr = ConnectionStringFactory.NXJCConnectionString;
        private static readonly ISqlServerDataFactory _dataFactory = new SqlServerDataFactory(_connStr);

        public static DataTable GetDowntimeReasonInfo()
        {
            string m_Sql = @"Select
                    A.ReasonItemID as ReasonItemId,  
                    rtrim(A.MachineHaltReasonID) as MachineHaltReasonId,
                    A.ReasonText as ReasonText 
                    from system_MachineHaltReason A 
                    where A.Enabled = 1";
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

        public static DataTable GetDowntimeTreatmentInfo(string myReasonItemId)
        {
            string m_Sql = @"Select
                    A.DowntimeTreatmentItemId as DowntimeTreatmentItemId,  
                    A.DowntimeTreatmentName as DowntimeTreatmentName,
                    B.ReasonText as ReasonText, 
                    C.USER_NAME as CreateName,
                    A.CreateTime as CreateTime,
                    A.Remarks as Remarks
                    from experience_DowntimeTreatment A 
                    left join system_MachineHaltReason B on A.ReasonItemID = B.ReasonItemID 
                    left join IndustryEnergy_SH.dbo.users C on A.Creator = C.USER_ID   
                    where A.ReasonItemID = '{0}' 
					order by A.CreateTime desc";
            m_Sql = string.Format(m_Sql, myReasonItemId);
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
        public static DataTable GetDowntimeTreatmentInfoById(string myDowntimeTreatmentItemId)
        {
            string m_Sql = @"Select
                    A.DowntimeTreatmentItemId as DowntimeTreatmentItemId,  
                    A.DowntimeTreatmentName as DowntimeTreatmentName,
                    A.ReasonItemID as ReasonItemId, 
                    A.Creator as Creator, 
                    A.CreateTime as CreateTime,
                    A.Remarks as Remarks
                    from experience_DowntimeTreatment A 
                    where A.DowntimeTreatmentItemId = '{0}'";
            m_Sql = string.Format(m_Sql, myDowntimeTreatmentItemId);
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
        public static string GetDowntimeTreatmentTextById(string myDowntimeTreatmentItemId)
        {
            string m_Sql = @"Select
                    A.Treatment as Treatment 
                    from experience_DowntimeTreatment A 
                    where A.DowntimeTreatmentItemId = '{0}'";
            m_Sql = string.Format(m_Sql, myDowntimeTreatmentItemId);
            try
            {
                DataTable m_Result = _dataFactory.Query(m_Sql);
                if (m_Result != null && m_Result.Rows.Count > 0)
                {
                    return m_Result.Rows[0]["Treatment"].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string GetDowntimePhenomenonTextById(string myDowntimeTreatmentItemId)
        {
            string m_Sql = @"Select
                    A.Phenomenon as Phenomenon 
                    from experience_DowntimeTreatment A 
                    where A.DowntimeTreatmentItemId = '{0}'";
            m_Sql = string.Format(m_Sql, myDowntimeTreatmentItemId);
            try
            {
                DataTable m_Result = _dataFactory.Query(m_Sql);
                if (m_Result != null && m_Result.Rows.Count > 0)
                {
                    return m_Result.Rows[0]["Phenomenon"].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetDowntimePhenomenonTreatmentTextById(string myDowntimeTreatmentItemId)
        {
            string m_Sql = @"Select
                    A.Phenomenon as Phenomenon,
                    A.Treatment as Treatment 
                    from experience_DowntimeTreatment A 
                    where A.DowntimeTreatmentItemId = '{0}'";
            m_Sql = string.Format(m_Sql, myDowntimeTreatmentItemId);
            try
            {
                DataTable m_Result = _dataFactory.Query(m_Sql);
                if (m_Result != null && m_Result.Rows.Count > 0)
                {
                    string m_Text = "<p>【故障描述】</p>" + m_Result.Rows[0]["Phenomenon"].ToString() + "<p>【故障处理】</p>" + m_Result.Rows[0]["Treatment"].ToString();
                    return m_Text;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static int AddDowntimeTreatment(string myDowntimeTreatmentName, string myReasonItemID, string myPhenomenon, string myTreatment,string myCreator, string myRemarks)
        {
            string m_Sql = @"Insert into experience_DowntimeTreatment 
                ( DowntimeTreatmentName, ReasonItemID, Phenomenon, Treatment, Creator, CreateTime, Remarks) 
                values
                ( @DowntimeTreatmentName, @ReasonItemID, @Phenomenon, @Treatment, @Creator, @CreateTime, @Remarks)";
            SqlParameter[] m_Parameters = { new SqlParameter("@DowntimeTreatmentName", myDowntimeTreatmentName),
                                          new SqlParameter("@ReasonItemID", myReasonItemID),
                                          new SqlParameter("@Phenomenon", myPhenomenon),
                                          new SqlParameter("@Treatment", myTreatment),
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
        public static int ModifyDowntimeTreatmentById(string myDowntimeTreatmentItemId, string myDowntimeTreatmentName, string myReasonItemID, string myPhenomenon, string myTreatment, string myCreator, string myRemarks)
        {
            string m_Sql = @"UPDATE experience_DowntimeTreatment SET
                            DowntimeTreatmentName = @DowntimeTreatmentName, 
                            ReasonItemID = @ReasonItemID, 
                            Phenomenon = @Phenomenon,
                            Treatment = @Treatment, 
                            Creator = @Creator,
                            CreateTime = @CreateTime,
                            Remarks = @Remarks
                            where DowntimeTreatmentItemId = @DowntimeTreatmentItemId";
            SqlParameter[] m_Parameters = { new SqlParameter("@DowntimeTreatmentItemId", myDowntimeTreatmentItemId),
                                          new SqlParameter("@DowntimeTreatmentName", myDowntimeTreatmentName),
                                          new SqlParameter("@ReasonItemID", myReasonItemID),
                                          new SqlParameter("@Phenomenon", myPhenomenon),
                                          new SqlParameter("@Treatment", myTreatment),
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
        public static int DeleteDowntimeTreatmentById(string myDowntimeTreatmentItemId)
        {
            string m_Sql = @"DELETE FROM experience_DowntimeTreatment where DowntimeTreatmentItemId=@DowntimeTreatmentItemId";
            SqlParameter[] m_Parameters = { new SqlParameter("@DowntimeTreatmentItemId", myDowntimeTreatmentItemId) };
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
