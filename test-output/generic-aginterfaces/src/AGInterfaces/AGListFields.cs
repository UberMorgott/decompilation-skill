using System.Drawing;

namespace AGInterfaces;

public class AGListFields : BaseDatasetFields
{
	public int SERNUM { get; set; }

	public string GROUP_NAME { get; set; }

	public string MODEL { get; set; }

	public string NUMBER { get; set; }

	public string ALIAS { get; set; }

	public string TELNUM { get; set; }

	public string ROUTE_NAME { get; set; }

	public string DRIVER_NAME { get; set; }

	public string DRIVER_ID { get; set; }

	public string IMPL_NAMES { get; set; }

	public string IMPL_IDS { get; set; }

	public int ID { get; set; }

	public int SHIFT_ID { get; set; }

	public string S_NAME { get; set; }

	public string S_ADDRESS { get; set; }

	public string S_ID { get; set; }

	public int S_TYPE { get; set; }

	public string S_CHANNEL { get; set; }

	public string S_LONGITUDE { get; set; }

	public string S_LATITUDE { get; set; }

	public double S_ALTITUDE { get; set; }

	public string S_REAL_TIME { get; set; }

	public string S_REG_TIME { get; set; }

	public string S_MOVE_TIME { get; set; }

	public string S_PARK_TIME { get; set; }

	public string S_PLAN_TIME { get; set; }

	public string E_NAME { get; set; }

	public string E_ADDRESS { get; set; }

	public string E_ID { get; set; }

	public int E_TYPE { get; set; }

	public string E_CHANNEL { get; set; }

	public string E_LONGITUDE { get; set; }

	public string E_LATITUDE { get; set; }

	public double E_ALTITUDE { get; set; }

	public string E_REAL_TIME { get; set; }

	public string E_REG_TIME { get; set; }

	public string E_MOVE_TIME { get; set; }

	public string E_PARK_TIME { get; set; }

	public string E_PLAN_TIME { get; set; }

	public double DISTANCE { get; set; }

	public double CM_DISTANCE { get; set; }

	public double PL_DISTANCE { get; set; }

	public int ROAD_QUAL { get; set; }

	public double DELTA_ALT { get; set; }

	public string MIN_CONT { get; set; }

	public string MAX_CONT { get; set; }

	public string REAL_CONT { get; set; }

	public string REG_CONT { get; set; }

	public string PLAN_CONT { get; set; }

	public string MOVE_CONT { get; set; }

	public string PARK_CONT { get; set; }

	public int PARK_CNTR { get; set; }

	public int GETIN_CNTR { get; set; }

	public int PLAN_CNTR { get; set; }

	public string PLAN_DELTA { get; set; }

	public double MAX_SPEED { get; set; }

	public double AVER_SPEED { get; set; }

	public int OS_COUNTER { get; set; }

	public double OS_DISTANCE { get; set; }

	public string OS_CONT { get; set; }

	public double TOTL_SQUARE { get; set; }

	public double CMPL_SQUARE { get; set; }

	public double OVER_SQUARE { get; set; }

	public double UNCM_SQUARE { get; set; }

	public int MAX_OVER { get; set; }

	public long COUNTER1 { get; set; }

	public long COUNTER2 { get; set; }

	public long COUNTER3 { get; set; }

	public long COUNTER4 { get; set; }

	public long COUNTER5 { get; set; }

	public long COUNTER6 { get; set; }

	public long COUNTER7 { get; set; }

	public long COUNTER8 { get; set; }

	public double T1_S_LEVEL { get; set; }

	public double T1_E_LEVEL { get; set; }

	public int T1_UP_CNTR { get; set; }

	public double T1_UP_VOL { get; set; }

	public int T1_DN_CNTR { get; set; }

	public double T1_DN_VOL { get; set; }

	public double T2_S_LEVEL { get; set; }

	public double T2_E_LEVEL { get; set; }

	public int T2_UP_CNTR { get; set; }

	public double T2_UP_VOL { get; set; }

	public int T2_DN_CNTR { get; set; }

	public double T2_DN_VOL { get; set; }

	public double T3_S_LEVEL { get; set; }

	public double T3_E_LEVEL { get; set; }

	public int T3_UP_CNTR { get; set; }

	public double T3_UP_VOL { get; set; }

	public int T3_DN_CNTR { get; set; }

	public double T3_DN_VOL { get; set; }

	public double T4_S_LEVEL { get; set; }

	public double T4_E_LEVEL { get; set; }

	public int T4_UP_CNTR { get; set; }

	public double T4_UP_VOL { get; set; }

	public int T4_DN_CNTR { get; set; }

	public double T4_DN_VOL { get; set; }

	public double M1_HOURS { get; set; }

	public double M1_M_HOURS { get; set; }

	public double M1_P_HOURS { get; set; }

	public double M1_WORK_MH { get; set; }

	public double M1_M_WRK_MH { get; set; }

	public double M1_P_WRK_MH { get; set; }

	public double M1_IDLE_MH { get; set; }

	public double M1_M_IDL_MH { get; set; }

	public double M1_P_IDL_MH { get; set; }

	public double M1_MAX_REV { get; set; }

	public double M1_AVER_REV { get; set; }

	public int M1_RV_COUNT { get; set; }

	public double M1_RV_DIST { get; set; }

	public string M1_RV_CONT { get; set; }

	public double M1_FUEL { get; set; }

	public double M1_FUEL_MOV { get; set; }

	public double M1_FUEL_PRK { get; set; }

	public double M1_FUEL_PLN { get; set; }

	public double M2_HOURS { get; set; }

	public double M2_M_HOURS { get; set; }

	public double M2_P_HOURS { get; set; }

	public double M2_WORK_MH { get; set; }

	public double M2_M_WRK_MH { get; set; }

	public double M2_P_WRK_MH { get; set; }

	public double M2_IDLE_MH { get; set; }

	public double M2_M_IDL_MH { get; set; }

	public double M2_P_IDL_MH { get; set; }

	public double M2_MAX_REV { get; set; }

	public double M2_AVER_REV { get; set; }

	public int M2_RV_COUNT { get; set; }

	public double M2_RV_DIST { get; set; }

	public string M2_RV_CONT { get; set; }

	public double M2_FUEL { get; set; }

	public double M2_FUEL_MOV { get; set; }

	public double M2_FUEL_PRK { get; set; }

	public double M2_FUEL_PLN { get; set; }

	public double M3_HOURS { get; set; }

	public double M3_M_HOURS { get; set; }

	public double M3_P_HOURS { get; set; }

	public double M3_WORK_MH { get; set; }

	public double M3_M_WRK_MH { get; set; }

	public double M3_P_WRK_MH { get; set; }

	public double M3_IDLE_MH { get; set; }

	public double M3_M_IDL_MH { get; set; }

	public double M3_P_IDL_MH { get; set; }

	public double M3_MAX_REV { get; set; }

	public double M3_AVER_REV { get; set; }

	public int M3_RV_COUNT { get; set; }

	public double M3_RV_DIST { get; set; }

	public string M3_RV_CONT { get; set; }

	public double M3_FUEL { get; set; }

	public double M3_FUEL_MOV { get; set; }

	public double M3_FUEL_PRK { get; set; }

	public double M3_FUEL_PLN { get; set; }

	public double M4_HOURS { get; set; }

	public double M4_M_HOURS { get; set; }

	public double M4_P_HOURS { get; set; }

	public double M4_WORK_MH { get; set; }

	public double M4_M_WRK_MH { get; set; }

	public double M4_P_WRK_MH { get; set; }

	public double M4_IDLE_MH { get; set; }

	public double M4_M_IDL_MH { get; set; }

	public double M4_P_IDL_MH { get; set; }

	public double M4_MAX_REV { get; set; }

	public double M4_AVER_REV { get; set; }

	public int M4_RV_COUNT { get; set; }

	public double M4_RV_DIST { get; set; }

	public string M4_RV_CONT { get; set; }

	public double M4_FUEL { get; set; }

	public double M4_FUEL_MOV { get; set; }

	public double M4_FUEL_PRK { get; set; }

	public double M4_FUEL_PLN { get; set; }

	public int B_COUNTER { get; set; }

	public double B_DISTANCE { get; set; }

	public string B_CONT { get; set; }

	public int R_COUNTER { get; set; }

	public double R_DISTANCE { get; set; }

	public string R_CONT { get; set; }

	public int T_COUNTER { get; set; }

	public double T_DISTANCE { get; set; }

	public string T_CONT { get; set; }

	public int V_COUNTER { get; set; }

	public double V_DISTANCE { get; set; }

	public string V_CONT { get; set; }

	public int TR_COUNTER { get; set; }

	public double TR_DISTANCE { get; set; }

	public string TR_CONT { get; set; }

	public string TR_S_TIME { get; set; }

	public double TR_S_DIST { get; set; }

	public string TR_E_TIME { get; set; }

	public double TR_E_DIST { get; set; }

	public int MR_COUNTER { get; set; }

	public double MR_DISTANCE { get; set; }

	public string MR_CONT { get; set; }

	public string MR_S_TIME { get; set; }

	public double MR_S_DIST { get; set; }

	public string MR_E_TIME { get; set; }

	public double MR_E_DIST { get; set; }

	public int I1_COUNTER { get; set; }

	public double I1_DISTANCE { get; set; }

	public string I1_CONT { get; set; }

	public string I1_S_TIME { get; set; }

	public double I1_S_DIST { get; set; }

	public string I1_E_TIME { get; set; }

	public double I1_E_DIST { get; set; }

	public int I2_COUNTER { get; set; }

	public double I2_DISTANCE { get; set; }

	public string I2_CONT { get; set; }

	public string I2_S_TIME { get; set; }

	public double I2_S_DIST { get; set; }

	public string I2_E_TIME { get; set; }

	public double I2_E_DIST { get; set; }

	public int I3_COUNTER { get; set; }

	public double I3_DISTANCE { get; set; }

	public string I3_CONT { get; set; }

	public string I3_S_TIME { get; set; }

	public double I3_S_DIST { get; set; }

	public string I3_E_TIME { get; set; }

	public double I3_E_DIST { get; set; }

	public int I4_COUNTER { get; set; }

	public double I4_DISTANCE { get; set; }

	public string I4_CONT { get; set; }

	public string I4_S_TIME { get; set; }

	public double I4_S_DIST { get; set; }

	public string I4_E_TIME { get; set; }

	public double I4_E_DIST { get; set; }

	public int I5_COUNTER { get; set; }

	public double I5_DISTANCE { get; set; }

	public string I5_CONT { get; set; }

	public string I5_S_TIME { get; set; }

	public double I5_S_DIST { get; set; }

	public string I5_E_TIME { get; set; }

	public double I5_E_DIST { get; set; }

	public int I6_COUNTER { get; set; }

	public double I6_DISTANCE { get; set; }

	public string I6_CONT { get; set; }

	public string I6_S_TIME { get; set; }

	public double I6_S_DIST { get; set; }

	public string I6_E_TIME { get; set; }

	public double I6_E_DIST { get; set; }

	public int I7_COUNTER { get; set; }

	public double I7_DISTANCE { get; set; }

	public string I7_CONT { get; set; }

	public string I7_S_TIME { get; set; }

	public double I7_S_DIST { get; set; }

	public string I7_E_TIME { get; set; }

	public double I7_E_DIST { get; set; }

	public int I8_COUNTER { get; set; }

	public double I8_DISTANCE { get; set; }

	public string I8_CONT { get; set; }

	public string I8_S_TIME { get; set; }

	public double I8_S_DIST { get; set; }

	public string I8_E_TIME { get; set; }

	public double I8_E_DIST { get; set; }

	public Image ROW_IMAGE { get; set; }
}
