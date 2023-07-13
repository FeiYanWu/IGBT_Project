using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IGBT_V2Helper
{
    public class IGBStructs
    {
        #region igbt_param struct
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct igbt_fix_para_t
        {
            public system_fix_para_t system_fix_para;
            public sequence_fix_para_t sequence_fix_para;
            public krw_fix_para_t krw_fix_para;
            public iges_fix_para_t iges_fix_para;
            public vgeth_fix_para_t vgeth_fix_para;
            public vcesat_fix_para_t vcesat_fix_para;
            public ices_fix_para_t ices_fix_para;
            public mpulse_fix_para_t mpulse_fix_para;
            public spulse_fix_para_t spulse_fix_para;
            public lshort_fix_para_t lshort_fix_para;
            public vces_fix_para_t vces_fix_para;
            public vf_fix_para_t vf_fix_para;
        }
        #endregion

        #region ices_param_struct
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct ices_public_fix_para_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 24)]
            public byte[] strategy;                             // 通道策略序列配置，strategy[0]代表第1次要测试第几个管子

            public byte resalt_error_is_continue;               // 测试结果出现故障时是否继续
            public uint hv_ce_wave_report_mask;                  // CE波形上报掩码，全部测试设置为0xFF即可
            public uint hv_ce_sampling_wave_time;                // CE采样波形时间，单位：us，1us采样50个点

            public double hv_ce_over_voltage_threshold;         // 高压脉冲板CE高压电压超限(比较器)
            public double hv_ce_over_current_threshold;         // 高压脉冲板CE高压电流超限(比较器)
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct ices_special_fix_para_t
        {
            public uint hv_ce_pulse_width;                   // 脉冲宽度，单位：us
            public double hv_ce_pulse_voltage;               // 脉冲电压幅值，单位V

            public byte hv_ce_curr_operat_ampli_sel;         // CE脉冲电流采集电阻选择，0: 10MΩ、1: 10KΩ、2:100Ω
            public byte hv_ce_curr_samp_resistor_sel;        // CE脉冲电流采集运放比例选择，0: 10倍、1: 1倍

            public double hv_ice_judge_down;                 // ices结果判定下限，单位A
            public double hv_ice_judge_up;                   // ices结果判定上限，单位A
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct ices_fix_para_t
        {
            public ices_public_fix_para_t ices_public_fix_para;

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
            public ices_special_fix_para_t[] ices_special_fix_para;
            public void Set(ices_public_fix_para_t public_fix_para, ices_special_fix_para_t special_fix_para)
            {
                ices_special_fix_para = new ices_special_fix_para_t[8];
                ices_public_fix_para = public_fix_para;

                for (int i = 0; i < 8; i++)
                {
                    ices_special_fix_para[i] = special_fix_para;
                }
            }
        }

        #endregion

        #region iges_para_struct

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct iges_public_fix_para_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 24)]
            public byte[] strategy;                             // 通道策略序列配置，strategy[0]代表第1次要测试第几个管子

            public byte resalt_error_is_continue;               // 测试结果出现故障时是否继续
            public uint smu_ge_wave_report_mask;                // 栅极波形上报掩码，全部上报设置为0xFF即可
            public uint smu_ge_wave_sampling_time;              // 栅极采样波形时间，单位：us
            public double smu_ge_over_voltage_threshold;        // 栅极电压保护点，单位：V
            public double smu_ge_over_current_threshold;        // 栅极漏电流保护点，单位：V
            public double smu_gear_voltage_up;                  // 跳档挡位电压上限	单位：V
            public double smu_gear_voltage_down;                // 跳档挡位电压下限	单位：V
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct iges_special_fix_para_t
        {

            public double smu_ge_positive_pulse_voltage;        // 正脉冲幅值,单位：V
            public uint smu_ge_positive_pulse_width;            // 正脉冲宽度,单位：us
            public double smu_ge_negative_pulse_voltage;        // 负脉冲幅值,单位：V
            public uint smu_ge_negative_pulse_width;            // 负脉冲宽度,单位：us

            public double smu_ige_judge_positive_down;          // 正向漏电流判据下限,单位：A
            public double smu_ige_judge_positive_up;            // 正向漏电流判据上限,单位：A
            public double smu_ige_judge_negative_down;          // 负向漏电流判据下限,单位：A
            public double smu_ige_judge_negative_up;            // 负向漏电流判据上限,单位：A
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct iges_fix_para_t
        {
            public iges_public_fix_para_t iges_public_fix_para;

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
            public iges_special_fix_para_t[] iges_special_fix_para;
            public void Set(iges_public_fix_para_t public_fix_para, iges_special_fix_para_t special_fix_para)
            {
                iges_special_fix_para = new iges_special_fix_para_t[8];
                iges_public_fix_para = public_fix_para;

                for (int i = 0; i < 8; i++)
                {
                    iges_special_fix_para[i] = special_fix_para;
                }
            }
        }

        #endregion

        #region krw_para_struct
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct krw_public_fix_para_t
        {
            public uint strategy;                                       // 通道策略配置，接触性策略(0-23bit)+温度策略(24-26bit)，全部测试设置为0x07FFFFFF即可
            public byte resalt_error_is_continue;                       // 测试结果出现故障时是否继续
            public uint wave_report_mask;                               // 波形上报掩码，接触性策略(0-23bit)+温度策略(24-26bit)，全部波形上传设置为0x07FFFFFF即可
            public uint wave_sampling_time;                             // 采样波形时间，单位us

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R8, SizeConst = 24)]
            public double[] contact_resistance_up;                      // 触点电阻(contact resistance)阻值上限，单位：Ω

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R8, SizeConst = 24)]
            public double[] contact_resistance_down;                    // 触点电阻(contact resistance)阻值下限[默认为0即可]，单位：Ω
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct krw_special_fix_para_t
        {
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct krw_fix_para_t
        {
            public krw_public_fix_para_t krw_public_fix_para;

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
            public krw_special_fix_para_t[] krw_special_fix_para;
            public void Set(krw_public_fix_para_t public_fix_para, krw_special_fix_para_t special_fix_para)
            {
                krw_special_fix_para = new krw_special_fix_para_t[8];
                krw_public_fix_para = public_fix_para;

                for (int i = 0; i < 8; i++)
                {
                    krw_special_fix_para[i] = special_fix_para;
                }
            }
        }


        #endregion

        #region lshort_para_struct

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct lshort_public_fix_para_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 24)]
            public byte[] strategy;  // 通道策略序列配置，strategy[0]代表第1次要测试第几个管子
            public byte resalt_error_is_continue;  // 测试结果出现故障时是否继续

            public double gd_a_pos_power_under_voltage_threshold;  // 栅极驱动板A路驱动电源欠压故障（+ 一路）（比较器）
            public double gd_a_neg_power_under_voltage_threshold;  // 栅极驱动板A路驱动电源欠压故障（- 一路）（比较器）
            public double gd_b_pos_power_under_voltage_threshold;  // 栅极驱动板B路驱动电源欠压故障（+ 一路）（比较器）
            public double gd_b_neg_power_under_voltage_threshold;  // 栅极驱动板B路驱动电源欠压故障（- 一路）（比较器）
            public double gd_a_drive_over_voltage_threshold;  // 栅极驱动板A路驱动过压故障（+一路）（比较器）
            public double gd_b_drive_over_voltage_threshold;  // 栅极驱动板B路驱动过压故障（+一路）（比较器）

            public double gp_b1_over_voltage_threshold;  // 栅极保护板B1 GE检测电压超限（比较器）
            public double gp_b2_over_voltage_threshold;  // 栅极保护板B2 GE检测电压超限（比较器）
            public double gp_b3_over_voltage_threshold;  // 栅极保护板B3 GE检测电压超限（比较器）
            public double gp_b4_over_voltage_threshold;  // 栅极保护板B4 GE检测电压超限（比较器）
            public double gp_a1_over_voltage_threshold;  // 栅极保护板A1 GE检测电压超限（比较器）
            public double gp_a2_over_voltage_threshold;  // 栅极保护板A2 GE检测电压超限（比较器）
            public double gp_a3_over_voltage_threshold;  // 栅极保护板A3 GE检测电压超限（比较器）
            public double gp_a4_over_voltage_threshold;  // 栅极保护板A4 GE检测电压超限（比较器）
            public uint gp_vge_detection_time;  // 栅极保护板栅极电压检测时间

            public double hv_vce_over_voltage_threshold;  // 高压脉冲板CE高压电压超限(比较器)
            public double hv_ice_over_current_threshold;  // 高压脉冲板CE高压电流超限(比较器)

            public double ccs_ch1_over_current_threshold;  // 恒流源板卡大电流输出过流1故障（比较器）
            public double ccs_ch2_over_current_threshold;  // 恒流源板卡大电流输出过流2故障（比较器）
            public double ccs_ch3_over_current_threshold;  // 恒流源板卡大电流输出过流3故障（比较器）
            public double ccs_ch4_over_current_threshold;  // 恒流源板卡大电流输出过流4故障（比较器）

            public uint gd_ge_on_resistance;  // 栅极驱动板导通电阻
            public uint gd_ge_off_resistance;  // 栅极驱动板关断电阻

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 6)]
            public byte[] rt02_load_inductance;  // 6个负载电感工作状态配置 1:有效 2：无效

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct lshort_special_fix_para_t
        {
            /****************************** 下面为动态测试时序配置参数（1：10ns） *************************/
            public uint rt01_trigger_calibration_time;  // 触发校准时间（多脉冲时序的校准时间，默认为0，主要解决硬件触发差异带来的小时间误差）
            public uint gd_trigger_calibration_time;  //触发校准时间（多脉冲时序的校准时间，默认为0，主要解决硬件触发差异带来的小时间误差）

            public uint rt01_secondary_capacitor_charge_start_time;  // 次级电容充电开始时间（默认为0，也为动态时序的0时刻）
            public uint rt01_secondary_capacitor_charge_completion_interval;  // 次级电容充电完成间隔（相对于双脉冲最后一个下降沿而言）
            public uint rt01_main_igbt_power_on_start_time;  // 打开主igbt供电回路开始时间
            public uint rt01_main_igbt_power_on_completion_interval;  // 打开主igbt供电回路完成间隔（相对于双脉冲最后一个下降沿而言）

            public uint rt01_oscilloscope_trigger_start_time;  // 示波器开始触发时间（相对于主igbt回路打开时刻而言）
            public uint rt01_oscilloscope_trigger_pulse1_width;  // 示波器触发脉冲1宽度
            public uint rt01_oscilloscope_trigger_pulse_interval;  // 示波器触发脉冲1和脉冲2中间的间隔
            public uint rt01_oscilloscope_trigger_pulse2_width;  // 示波器触发脉冲2宽度
            public uint rt01_oscilloscope_trigger_pulse2_repeat_count;  // 示波器触发脉冲重复次数

            public uint gd_double_pulse_trigger_start_time;  // 栅极驱动板双脉冲触发时间（相对于主igbt回路打开时刻而言）
            public uint gd_double_pulse_trigger_pulse1_width;  // 栅极驱动板双脉冲_脉冲1宽度
            public uint gd_double_pulse_trigger_pulse_interval;  // 栅极驱动板双脉冲_脉冲1和脉冲2中间的间隔
            public uint gd_double_pulse_trigger_pulse2_width;  // 栅极驱动板双脉冲_脉冲2宽度
            public uint gd_double_pulse_trigger_pulse2_repeat_count;  // 栅极驱动板双脉冲_脉冲2冲重复次数
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct lshort_fix_para_t
        {
            public lshort_public_fix_para_t lshort_public_fix_para;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
            public lshort_special_fix_para_t[] lshort_special_fix_para;
            public void Set(lshort_public_fix_para_t public_fix_para, lshort_special_fix_para_t special_fix_para)
            {
                lshort_special_fix_para = new lshort_special_fix_para_t[8];
                lshort_public_fix_para = public_fix_para;

                for (int i = 0; i < 8; i++)
                {
                    lshort_special_fix_para[i] = special_fix_para;
                }
            }
        }
        #endregion

        #region mpulse_para_struct

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct mpulse_public_fix_para_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 24)]
            public byte[] strategy;
            public byte resalt_error_is_continue;
            public double gd_a_pos_power_under_voltage_threshold;
            public double gd_a_neg_power_under_voltage_threshold;
            public double gd_b_pos_power_under_voltage_threshold;
            public double gd_b_neg_power_under_voltage_threshold;
            public double gd_a_drive_over_voltage_threshold;
            public double gd_b_drive_over_voltage_threshold;
            public double gp_b1_over_voltage_threshold;
            public double gp_b2_over_voltage_threshold;
            public double gp_b3_over_voltage_threshold;
            public double gp_b4_over_voltage_threshold;
            public double gp_a1_over_voltage_threshold;
            public double gp_a2_over_voltage_threshold;
            public double gp_a3_over_voltage_threshold;
            public double gp_a4_over_voltage_threshold;
            public uint gp_vge_detection_time;
            public double hv_vce_over_voltage_threshold;
            public double hv_ice_over_current_threshold;
            public double ccs_ch1_over_current_threshold;
            public double ccs_ch2_over_current_threshold;
            public double ccs_ch3_over_current_threshold;
            public double ccs_ch4_over_current_threshold;
            public uint gd_ge_on_resistance;
            public uint gd_ge_off_resistance;

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 6)]
            public byte[] rt02_load_inductance;  // 6个负载电感工作状态配置 1:有效 2：无效
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct mpulse_special_fix_para_t
        {
            public uint rt01_trigger_calibration_time;
            public uint gd_trigger_calibration_time;
            public uint rt01_secondary_capacitor_charge_start_time;
            public uint rt01_secondary_capacitor_charge_completion_interval;
            public uint rt01_main_igbt_power_on_start_time;
            public uint rt01_main_igbt_power_on_completion_interval;
            public uint rt01_oscilloscope_trigger_start_time;
            public uint rt01_oscilloscope_trigger_pulse1_width;
            public uint rt01_oscilloscope_trigger_pulse_interval;
            public uint rt01_oscilloscope_trigger_pulse2_width;
            public uint rt01_oscilloscope_trigger_pulse2_repeat_count;
            public uint gd_double_pulse_trigger_start_time;
            public uint gd_double_pulse_trigger_pulse1_width;
            public uint gd_double_pulse_trigger_pulse_interval;
            public uint gd_double_pulse_trigger_pulse2_width;
            public uint gd_double_pulse_trigger_pulse2_repeat_count;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct mpulse_fix_para_t
        {
            public mpulse_public_fix_para_t mpulse_public_fix_para;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
            public mpulse_special_fix_para_t[] mpulse_special_fix_para;
            public void Set(mpulse_public_fix_para_t public_fix_para, mpulse_special_fix_para_t special_fix_para)
            {
                mpulse_special_fix_para = new mpulse_special_fix_para_t[8];
                mpulse_public_fix_para = public_fix_para;

                for (int i = 0; i < 8; i++)
                {
                    mpulse_special_fix_para[i] = special_fix_para;
                }
            }
        }

        #endregion

        #region sequence_para_struct

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct sequence_public_fix_para_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 24)]
            public byte[] sequence;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct sequence_special_fix_para_t
        {

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct sequence_fix_para_t
        {
            public sequence_public_fix_para_t sequence_public_fix_para;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
            public sequence_special_fix_para_t[] sequence_special_fix_para;
            public void Set(sequence_public_fix_para_t public_fix_para, sequence_special_fix_para_t special_fix_para)
            {
                sequence_special_fix_para = new sequence_special_fix_para_t[8];
                sequence_public_fix_para = public_fix_para;

                for (int i = 0; i < 8; i++)
                {
                    sequence_special_fix_para[i] = special_fix_para;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct sequence_public_dynamics_para_t
        {

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct sequence_special_dynamics_para_t
        {

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct sequence_dynamics_para_t
        {
            public sequence_public_dynamics_para_t sequence_public_dynamics_para;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
            public sequence_special_dynamics_para_t[] sequence_special_dynamics_para;
        }

        #endregion

        #region spulse_para_struct

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct spulse_public_fix_para_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 24)]
            public byte[] strategy;
            public byte resalt_error_is_continue;

            public double gd_a_pos_power_under_voltage_threshold;
            public double gd_a_neg_power_under_voltage_threshold;
            public double gd_b_pos_power_under_voltage_threshold;
            public double gd_b_neg_power_under_voltage_threshold;
            public double gd_a_drive_over_voltage_threshold;
            public double gd_b_drive_over_voltage_threshold;
            public double gp_b1_over_voltage_threshold;
            public double gp_b2_over_voltage_threshold;
            public double gp_b3_over_voltage_threshold;
            public double gp_b4_over_voltage_threshold;
            public double gp_a1_over_voltage_threshold;
            public double gp_a2_over_voltage_threshold;
            public double gp_a3_over_voltage_threshold;
            public double gp_a4_over_voltage_threshold;
            public uint gp_vge_detection_time;
            public double hv_vce_over_voltage_threshold;
            public double hv_ice_over_current_threshold;
            public double ccs_ch1_over_current_threshold;
            public double ccs_ch2_over_current_threshold;
            public double ccs_ch3_over_current_threshold;
            public double ccs_ch4_over_current_threshold;
            public uint gd_ge_on_resistance;
            public uint gd_ge_off_resistance;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 6)]
            public byte[] rt02_load_inductance;  // 6个负载电感工作状态配置 1:有效 2：无效
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct spulse_special_fix_para_t
        {
            public uint rt01_trigger_calibration_time;
            public uint gd_trigger_calibration_time;
            public uint rt01_secondary_capacitor_charge_start_time;
            public uint rt01_secondary_capacitor_charge_completion_interval;
            public uint rt01_main_igbt_power_on_start_time;
            public uint rt01_main_igbt_power_on_completion_interval;
            public uint rt01_oscilloscope_trigger_start_time;
            public uint rt01_oscilloscope_trigger_pulse1_width;
            public uint rt01_oscilloscope_trigger_pulse_interval;
            public uint rt01_oscilloscope_trigger_pulse2_width;
            public uint rt01_oscilloscope_trigger_pulse2_repeat_count;
            public uint gd_double_pulse_trigger_start_time;
            public uint gd_double_pulse_trigger_pulse1_width;
            public uint gd_double_pulse_trigger_pulse_interval;
            public uint gd_double_pulse_trigger_pulse2_width;
            public uint gd_double_pulse_trigger_pulse2_repeat_count;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct spulse_fix_para_t
        {
            public spulse_public_fix_para_t spulse_public_fix_para;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
            public spulse_special_fix_para_t[] spulse_special_fix_para;
            public void Set(spulse_public_fix_para_t public_fix_para, spulse_special_fix_para_t special_fix_para)
            {
                spulse_special_fix_para = new spulse_special_fix_para_t[8];
                spulse_public_fix_para = public_fix_para;

                for (int i = 0; i < 8; i++)
                {
                    spulse_special_fix_para[i] = special_fix_para;
                }
            }
        }

        #endregion

        #region system_para_struct
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct system_fix_para_t
        {
            public uint main_cap_charge_voltage;
            public uint main_cap_charge_voltage_accuracy;
            public uint main_cap_charge_keep_time;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
            public byte[] smu_channel_map; // SMU板通道映射
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
            public byte[] gp_channel_map;     // 栅极保护板通道映射
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
            public byte[] gd_channel_map;     // 栅极驱动板通道映射
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
            public byte[] hv_channel_map;     // 高压脉冲板通道映射
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
            public byte[] ccs_channel_map;    // 恒流源板通道映射
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
            public byte[] rt1_channel_map;    // rt1板通道映射
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
            public byte[] rt2_channel_map;    // rt2板通道映射
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
            public byte[] bridge_site;
        }
        #endregion

        #region vces_para_struct

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vces_public_fix_para_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 24)]
            public byte[] strategy;
            public byte resalt_error_is_continue;
            public uint hv_ce_wave_report_mask;
            public uint hv_ce_sampling_wave_time;
            public double hv_ce_over_voltage_threshold;
            public double hv_ce_over_current_threshold;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vces_special_fix_para_t
        {
            public double hv_vce_targer_current; // vce目标电流，单位A
            public double hv_vce_start_voltage; // vce起始电压，单位V
            public double hv_vce_max_voltage; // vce最大电压，单位V

            public byte hv_ce_curr_operat_ampli_sel; // CE脉冲电流采集电阻选择，0: 10MQ、1: 10KQ、2:1Q
            public byte hv_ce_curr_samp_resistor_sel; // CE脉冲电流采集运放比例选择，0: 10倍、1: 2倍

            public double hv_vce_judge_down; // vces结果判定下限，单位A
            public double hv_vce_judge_up; // vces结果判定上限，单位A
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vces_fix_para_t
        {
            public vces_public_fix_para_t vces_public_fix_para;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
            public vces_special_fix_para_t[] vces_special_fix_para;
            public void Set(vces_public_fix_para_t public_fix_para, vces_special_fix_para_t special_fix_para)
            {
                vces_special_fix_para = new vces_special_fix_para_t[8];
                vces_public_fix_para = public_fix_para;

                for (int i = 0; i < 8; i++)
                {
                    vces_special_fix_para[i] = special_fix_para;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vces_public_dynamics_para_t
        {
            // Define the fields for public dynamics parameters if any
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vces_special_dynamics_para_t
        {
            // Define the fields for special dynamics parameters if any
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vces_dynamics_para_t
        {
            public vces_public_dynamics_para_t vces_public_dynamics_para;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
            public vces_special_dynamics_para_t[] vces_special_dynamics_para;
        }

        #endregion

        #region vcesat_para_struct
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vcesat_public_fix_para_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 24)]
            public byte[] strategy;
            public byte resalt_error_is_continue;
            public uint hv_ce_wave_report_mask;
            public uint hv_ce_wave_sampling_time;
            public double gd_a_pos_power_under_voltage_threshold;
            public double gd_a_neg_power_under_voltage_threshold;
            public double gd_b_pos_power_under_voltage_threshold;
            public double gd_b_neg_power_under_voltage_threshold;
            public double gd_a_drive_over_voltage_threshold;
            public double gd_b_drive_over_voltage_threshold;
            public double gp_b1_over_voltage_threshold;
            public double gp_b2_over_voltage_threshold;
            public double gp_b3_over_voltage_threshold;
            public double gp_b4_over_voltage_threshold;
            public double gp_a1_over_voltage_threshold;
            public double gp_a2_over_voltage_threshold;
            public double gp_a3_over_voltage_threshold;
            public double gp_a4_over_voltage_threshold;
            public uint gp_vge_detection_time;
            public double hv_vce_over_voltage_threshold;
            public double hv_ice_over_current_threshold;
            public double ccs_capac_charg_volt_down;
            public double ccs_capac_charg_volt_up;
            public double ccs_ch1_over_current_threshold;
            public double ccs_ch2_over_current_threshold;
            public double ccs_ch3_over_current_threshold;
            public double ccs_ch4_over_current_threshold;

            public uint gd_ge_on_resistance;
            public uint gd_ge_off_resistance;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vcesat_special_fix_para_t
        {
            public uint ccs_output_pulse_width;          // 恒流源输出脉冲宽度，单位：us
            public double ccs_output_current_value;      // 恒流源输出电流值，单位：A

            public double ccs_ice_judge_down;            // 恒流源结果判据下限
            public double ccs_ice_judge_up;              // 恒流源结果判据上限

            public uint trigger_calibration_time;        // 触发校准时间（多脉冲时序的校准时间，默认为0，主要解决硬件触发差异带来的小时间误差）
            public uint hv_trigger_time;                 // 高压脉冲板触发多长时间后开启采集（单位：us，0:立即输出）

            public double hv_vce_judge_down;             // 高压脉冲板结果判据下限
            public double hv_vce_judge_up;               // 高压脉冲板结果判据上限
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vcesat_fix_para_t
        {
            public vcesat_public_fix_para_t vcesat_public_fix_para;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
            public vcesat_special_fix_para_t[] vcesat_special_fix_para;
            public void Set(vcesat_public_fix_para_t public_fix_para, vcesat_special_fix_para_t special_fix_para)
            {
                vcesat_special_fix_para = new vcesat_special_fix_para_t[8];
                vcesat_public_fix_para = public_fix_para;

                for (int i = 0; i < 8; i++)
                {
                    vcesat_special_fix_para[i] = special_fix_para;
                }
            }
        }

        #endregion

        #region vf_para_struct

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vf_public_fix_para_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 24)]
            public byte[] strategy;                             // 通道策略序列配置，strategy[0]代表第1次要测试第几个管子
            public byte resalt_error_is_continue;               // 测试结果出现故障时是否继续

            public uint hv_ce_wave_report_mask;                 // CE波形上报掩码，全部测试设置为0xFF即可
            public uint hv_ce_wave_sampling_time;               // CE采样波形时间，单位：us，1us采样50个点

            public double gd_a_pos_power_under_voltage_threshold;       // 栅极驱动板A路驱动电源欠压故障（+ 一路）（比较器）
            public double gd_a_neg_power_under_voltage_threshold;       // 栅极驱动板A路驱动电源欠压故障（- 一路）（比较器）
            public double gd_b_pos_power_under_voltage_threshold;       // 栅极驱动板B路驱动电源欠压故障（+ 一路）（比较器）
            public double gd_b_neg_power_under_voltage_threshold;       // 栅极驱动板B路驱动电源欠压故障（- 一路）（比较器）
            public double gd_a_drive_over_voltage_threshold;            // 栅极驱动板A路驱动过压故障（+一路）（比较器）
            public double gd_b_drive_over_voltage_threshold;            // 栅极驱动板B路驱动过压故障（+一路）（比较器）

            public double gp_b1_over_voltage_threshold;           // 栅极保护板B1 GE检测电压超限（比较器）
            public double gp_b2_over_voltage_threshold;           // 栅极保护板B2 GE检测电压超限（比较器）
            public double gp_b3_over_voltage_threshold;           // 栅极保护板B3 GE检测电压超限（比较器）
            public double gp_b4_over_voltage_threshold;           // 栅极保护板B4 GE检测电压超限（比较器）
            public double gp_a1_over_voltage_threshold;           // 栅极保护板A1 GE检测电压超限（比较器）
            public double gp_a2_over_voltage_threshold;           // 栅极保护板A2 GE检测电压超限（比较器）
            public double gp_a3_over_voltage_threshold;           // 栅极保护板A3 GE检测电压超限（比较器）
            public double gp_a4_over_voltage_threshold;           // 栅极保护板A4 GE检测电压超限（比较器）
            public uint gp_vge_detection_time;                    // 栅极保护板栅极电压检测时间

            public double hv_vce_over_voltage_threshold;          // 高压脉冲板CE高压电压超限(比较器)
            public double hv_ice_over_current_threshold;          // 高压脉冲板CE高压电流超限(比较器)

            public double ccs_capac_charg_volt_down;              // 恒流源电容充电电压下限
            public double ccs_capac_charg_volt_up;                // 恒流源电容充电电压上限
            public double ccs_ch1_over_current_threshold;         // 恒流源板卡大电流输出过流1故障（比较器）
            public double ccs_ch2_over_current_threshold;         // 恒流源板卡大电流输出过流2故障（比较器）
            public double ccs_ch3_over_current_threshold;         // 恒流源板卡大电流输出过流3故障（比较器）
            public double ccs_ch4_over_current_threshold;         // 恒流源板卡大电流输出过流4故障（比较器）

            public uint gd_ge_on_resistance;                             // 栅极驱动板导通电阻
            public uint gd_ge_off_resistance;                             // 栅极驱动板关断电阻	
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vf_special_fix_para_t
        {
            public uint ccs_output_pulse_width;          // 恒流源输出脉冲宽度，单位：us
            public double ccs_output_current_value;      // 恒流源输出电流值，单位：A

            public double ccs_ice_judge_down;            // 恒流源结果判据下限
            public double ccs_ice_judge_up;              // 恒流源结果判据上限

            public uint trigger_calibration_time;        // 触发校准时间（多脉冲时序的校准时间，默认为0，主要解决硬件触发差异带来的小时间误差）
            public uint hv_trigger_time;                 // 高压脉冲板触发多长时间后开启采集（单位：us，0:立即输出）

            public double hv_vce_judge_down;             // 高压脉冲板结果判据下限
            public double hv_vce_judge_up;               // 高压脉冲板结果判据上限
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vf_fix_para_t
        {
            public vf_public_fix_para_t vf_public_fix_para;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
            public vf_special_fix_para_t[] vf_special_fix_para;
            public void Set(vf_public_fix_para_t public_fix_para, vf_special_fix_para_t special_fix_para)
            {
                vf_special_fix_para = new vf_special_fix_para_t[8];
                vf_public_fix_para = public_fix_para;

                for (int i = 0; i < 8; i++)
                {
                    vf_special_fix_para[i] = special_fix_para;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vf_public_dynamics_para_t
        {

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vf_special_dynamics_para_t
        {

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vf_dynamics_para_t
        {
            public vf_public_dynamics_para_t vf_public_dynamics_para;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
            public vf_special_dynamics_para_t[] vf_special_dynamics_para;
        }

        #endregion

        #region vgeth_para_struct

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vgeth_public_fix_para_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 24)]
            public byte[] strategy;
            public byte resalt_error_is_continue;

            public uint smu_ge_wave_report_mask;
            public uint smu_ge_sampling_wave_time;
            public double smu_ge_over_voltage_threshold;
            public double smu_ce_over_current_threshold;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vgeth_special_fix_para_t
        {
            public double smu_ge_current;

            public double smu_ge_start_voltage;
            public double smu_ge_end_voltage;
            public uint smu_ge_ramp_width;

            public double smu_vge_judge_down;
            public double smu_vge_judge_up;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct vgeth_fix_para_t
        {
            public vgeth_public_fix_para_t vgeth_public_fix_para;

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 8)]
            public vgeth_special_fix_para_t[] vgeth_special_fix_para;
            public void Set(vgeth_public_fix_para_t public_fix_para, vgeth_special_fix_para_t special_fix_para)
            {
                vgeth_special_fix_para = new vgeth_special_fix_para_t[8];
                vgeth_public_fix_para = public_fix_para;

                for (int i = 0; i < 8; i++)
                {
                    vgeth_special_fix_para[i] = special_fix_para;
                }
            }
        }


        #endregion

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct cbb_flow_state_t
        {
            public byte test_item_state;
            public byte test_item_state_para;
            public byte main_flow_state;
            public byte child_flow_state;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct oscilloscope_state_t
        {
            public byte is_valid;
            public byte test_item;
            public byte test_state_add_para;
        }

        #region fault_struct
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct soft_fault_t
        {
            public byte status;
            public byte type;
            public uint code;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct fault_info_t
        {
            public uint hw_fault1;
            public uint hw_fault2;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 5)]
            public soft_fault_t[] soft_fault;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct fault_list_item_t
        {
            public byte hw_fault1;
            public byte hw_fault2;
            public byte soft_fault;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct fault_list_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 9)]
            public fault_list_item_t[] fault_list_item;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct cbb_fault_para_t
        {
            public uint hw_fault1_disable;
            public uint hw_fault2_disable;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = 32)]
            public uint[] hw_fault1_filter;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = 32)]
            public uint[] hw_fault2_filter;
        }
        #endregion

        #region rt1000_struct

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct cbb_rt1000_para_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R8, SizeConst = 8)]
            public double[] adc_coefficient;                                // adc系数，默认为1
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R8, SizeConst = 8)]
            public double[] adc_zero_calibration;                           // adc零位校准（默认为0）
            public byte adc_upload_en;                                      // adc上传使能（默认不使能）
            public uint adc_upload_time;                                    // adc上传时间（默认100ms）

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R8, SizeConst = 8)]
            public double[] dac_coefficient;                                // dac系数，默认为1
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R8, SizeConst = 8)]
            public double[] dac_default_output;                             // dac上电默认输出（默认5V，规则：设定值*dac系数）

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 24)]
            public byte[] rt_t2_01_do;                                      // 第1块T2板卡DO上电默认值设置
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 24)]
            public byte[] rt_t2_02_do;                                      // 第2块T2板卡DO上电默认值设置
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct rt1000_info_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 64)]
            public byte[] rt_t2_01_di;                                       // 第1块T2板卡DI采集值
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 64)]
            public byte[] rt_t2_02_di;                                       // 第2块T2板卡DI采集值

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U8, SizeConst = 8)]
            public double[] adc_upload_data;                                 // adc上传数据（规则：采集值*adc系数）
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct charge_state_t
        {
            public byte is_success;                         // 是否成功
            public byte fail_code;                          // 失败代码(0：无效 1：主级电压异常 2：次级电压异常 3：主级次级都异常 4：气缸错误)
        }

        #endregion

        #region result
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct result_krw_t
        {
            public byte is_success;                    // 错误：0x00 正常：0x01
            public byte err_ch;                        // 错误通道号
            public double err_cr_value;                 // 错误电阻值
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U8, SizeConst = 24)]
            public double[] resistance_value;           // 24路电阻值
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U8, SizeConst = 3)]
            public double[] temper_value;               // 三路温度值
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct result_iges_t
        {
            public byte channel;
            public byte is_success;                    // 错误：0x00 正常：0x01
            public double positive_current;           // 正向采集电流
            public double negative_current;           // 负向采集电流
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct result_vgeth_t
        {
            public byte channel;
            public byte is_success;                    // 错误：0x00 正常：0x01
            public double vgeth_current;              // vge电流
            public double vgeth;                      // vgeth阀值电压
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct result_vcesat_t
        {
            public byte channel;
            public byte is_success;
            public double ce_voltage;
            public double ce_current;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct result_ices_t
        {
            public byte channel;
            public byte is_success;
            public double ce_voltage;
            public double ce_current;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct result_mpulse_t
        {
            // Empty struct
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct result_spulse_t
        {
            // Empty struct
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct result_lshort_t
        {
            // Empty struct
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct result_vces_t
        {
            public byte channel;
            public byte is_success;
            public double ce_voltage;
            public double ce_current;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct result_vf_t
        {
            public byte channel;
            public byte is_success;
            public double ce_voltage;
            public double ce_current;
        }
        #endregion

        #region wluart
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct cbb_wluart_para_t
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = 8)]
            public uint[] baud;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct uart_frame_t
        {
            public uint length;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 20)]
            public byte[] datas;
        }
        #endregion
    }
}
