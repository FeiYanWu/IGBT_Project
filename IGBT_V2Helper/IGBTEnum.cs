using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGBT_V2Helper
{
    /// <summary>
    /// 板卡ID枚举体
    /// </summary>
    public enum HardwareEnum
    {
        HW_MANAGE = 0,
        HW_KRW,
        HW_SMU,
        HW_HV,
        HW_GD,
        HW_CCS,
        HW_GP,
        HW_RT1000_01,
        HW_RT1000_02,
        HW_MAX
    }

    /// <summary>
    /// 板卡地址枚举体
    /// </summary>
    public enum HardwareAddressEnum
    {
        HW_ADDR_MANAGE = 0,
        HW_ADDR_KRW,
        HW_ADDR_SMU,
        HW_ADDR_HV,
        HW_ADDR_GD,
        HW_ADDR_CCS,
        HW_ADDR_GP,
        HW_ADDR_RT1000_01,
        HW_ADDR_RT1000_02,
        HW_ADDR_MAX
    }

    /// <summary>
    /// 测试项枚举体
    /// </summary>
    public enum TestItemsEnum
    {
        ITEM_RESERVE0 = 0,
        ITEM_KRW = 1,
        ITEM_RESERVE1 = 2,
        ITEM_IGES = 3,
        ITEM_VGETH = 4,
        ITEM_VCESAT = 5,
        ITEM_ICES = 6,
        ITEM_MPULSE = 7,
        ITEM_SPULSE = 8,
        ITEM_LSHORT = 9,
        ITEM_VCES = 10,
        ITEM_VF = 11
    }


    /// <summary>
    /// 功能码二枚举
    /// </summary>
    public enum FunctionCodeEnum
    {
        FUNC_TEST_CONFIG = 1,
        FUNC_TEST_READY,
        FUNC_TEST_START,
        FUNC_TEST_FINISH,
        FUNC_TEST_RESULT_RETURN,
        FUNC_TEST_CHANNEL_FINISH,
        FUNC_TEST_RESULT_FOEWARD,
        FUNC_TEST_RESPONSE = 0xFF
    }

    public enum WaveKrwIDEnum
    {

    }

    public enum WaveManageIDEnum
    {

    }

    public enum WaveCcsIDEnum
    {

    }

    public enum WaveHvIDEnum
    {
        MAX_WAVE
    }

    /// <summary>
    /// CE脉冲输出选择，0：未输出、1：输出正向
    /// </summary>
    public enum HvCePulseModeSelEnum
    {
        NoOutput = 0,
        ForwardOutput
    }

    /// <summary>
    /// ICES_CE脉冲电流采集电阻选择，0: 10MQ、1: 10KQ、2:100Q
    /// </summary>
    public enum ICESHvCeCurrOperatAmpliSelEnum
    {
        TenM = 0,
        TenK = 1,
        OneHund = 2
    }
    /// <summary>
    /// ICES_CE脉冲电流采集运放比例选择，0: 10倍、1: 1倍
    /// </summary>
    public enum ICESHvCeCurrSampResistorSelEnum
    {
        TenFold = 0,
        OneFold = 1
    }

    /// <summary>
    /// 6个负载电感工作状态配置 1:有效 2：无效
    /// </summary>
    public enum Rt02LoadInductanceEnum
    {
        Valid = 0,
        Invalid = 1
    }

    /// <summary>
    /// 8个管子的桥位配置（0：上桥 1：下桥）
    /// </summary>
    public enum BridgeSiteEnum
    {
        OnBridge = 0,
        OffBridge = 1
    }

    /// <summary>
    /// VCES_CE脉冲电流采集电阻选择，0: 10MQ、1: 10KQ、2:1Q
    /// </summary>
    public enum VCESHvCeCurrOperatAmpliSelEnum
    {
        TenM = 0,
        TenK = 1,
        OneHun = 2
    }
    /// <summary>
    /// VCES_CE脉冲电流采集运放比例选择，0: 10倍、1: 2倍
    /// </summary>
    public enum VCESHvCeCurrSampResistorSelEnum
    {
        TenFold = 0,
        OneFold = 1
    }
    /// <summary>
    /// 
    /// 电感枚举
    /// 
    /// </summary>
    public enum InductanceEnum
    {
        Inductance20,
        Inductance50,
        Inductance100,
        Inductance200,
        Inductance500,
        Inductance1000,
    }
}
