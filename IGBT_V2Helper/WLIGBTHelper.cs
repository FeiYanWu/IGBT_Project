using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static IGBT_V2Helper.IGBStructs;

namespace IGBT_V2Helper
{
    public class WLIGBTHelper
    {
        private const string DllPath = "WLIGBT\\WL_IGBT_C01.dll";

        #region 设备通用API规范
        /// <summary>
        /// 打开C01
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="handle">句柄</param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "WLIGBT_C01_Open", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WLIGBT_C01_Open([InAttribute][MarshalAsAttribute(UnmanagedType.LPStr)] string ip, ref uint handle);


        /// <summary>
        /// 关闭C01
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "WLIGBT_C01_Close", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WLIGBT_C01_Close(uint handle);


        /// <summary>
        /// 重置C01
        /// </summary>
        /// <param name="handle">句柄</param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "WLIGBT_C01_Reset", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WLIGBT_C01_Reset(uint handle);
        #endregion

        #region param
        [DllImportAttribute(DllPath, EntryPoint = "cbb_para_set_para", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_para_set_para(uint handle, ref igbt_fix_para_t igbt_fix_para);

        [DllImportAttribute(DllPath, EntryPoint = "cbb_para_get_para", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_para_get_para(uint handle, ref igbt_fix_para_t igbt_fix_para);
        #endregion

        #region fault
        /// <summary>
        /// 获取当前故障信息
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fault_info"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "cbb_fault_get_current_fault_info", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_fault_get_current_fault_info(uint handle, ref fault_info_t fault_info);


        /// <summary>
        /// 获取当前故障清单
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fault_list"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "cbb_fault_get_current_fault_list", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_fault_get_current_fault_list(uint handle, ref fault_list_t fault_list);

        /// <summary>
        /// 故障参数设置
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fault_para"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "cbb_fault_set_para", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_fault_set_para(uint handle, ref cbb_fault_para_t fault_para);


        /// <summary>
        /// 故障参数获取 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="fault_para"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "cbb_fault_get_para", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_fault_get_para(uint handle, ref cbb_fault_para_t fault_para);


        /// <summary>
        /// 清除硬件板卡所有故障
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "cbb_fault_clear", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_fault_clear(uint handle);


        /// <summary>
        /// 获取故障信息历史长度
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "cbb_fault_get_fault_info_history_length", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_fault_get_fault_info_history_length(uint handle, ref uint length);


        /// <summary>
        /// 获取故障信息历史
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="history_fault_info"></param>
        /// <param name="length"></param>
        /// <param name="real_length"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "cbb_fault_get_fault_info_history", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_fault_get_fault_info_history(uint handle, [MarshalAs(UnmanagedType.LPArray)] fault_info_t[] history_fault_info, uint length, ref uint real_length);


        /// <summary>
        /// 获取故障清单历史长度
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "cbb_fault_get_fault_list_history_length", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_fault_get_fault_list_history_length(uint handle, ref uint length);


        /// <summary>
        /// 获取故障清单历史
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="history_fault_list"></param>
        /// <param name="length"></param>
        /// <param name="real_length"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "cbb_fault_get_fault_list_history", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_fault_get_fault_list_history(uint handle, [MarshalAs(UnmanagedType.LPArray)] fault_list_t[] history_fault_list, uint length, ref uint real_length);

        #endregion

        #region result

        // 声明函数
        [DllImportAttribute(DllPath, EntryPoint = "cbb_result_read_length", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_result_read_length(uint handle, TestItemsEnum item_type, ref uint length);

        //[DllImportAttribute(DllPath, EntryPoint = "cbb_result_krw_read", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int cbb_result_krw_read(uint handle, ref result_krw_t result_krw, uint length, ref uint real_length);

        [DllImportAttribute(DllPath, EntryPoint = "ccb_result_clear", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ccb_result_clear(uint handle, TestItemsEnum item_type);

        [DllImportAttribute(DllPath, EntryPoint = "cbb_result_clear_all", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_result_clear_all(uint handle);

        [DllImportAttribute(DllPath, EntryPoint = "cbb_result_iges_read", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_result_iges_read(uint handle, ref result_iges_t result_iges, uint length, ref uint real_length);

        [DllImportAttribute(DllPath, EntryPoint = "cbb_result_vgeth_read", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_result_vgeth_read(uint handle, ref result_vgeth_t result_vgeth, uint length, ref uint real_length);


        #endregion

        #region cbb_flow
        /// <summary>
        /// 开始（从flash） 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "wl_igbt_flow_start", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_flow_start(uint handle);


        /// <summary>
        /// 停止	
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "wl_igbt_flow_stop", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_flow_stop(uint handle);


        /// <summary>
        /// 开始（从上位机参数）
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="items"></param>
        /// <param name="krw"></param>
        /// <param name="iges"></param>
        /// <param name="vgeth"></param>
        /// <param name="vcesat"></param>
        /// <param name="ices"></param>
        /// <param name="mpulse"></param>
        /// <param name="spulse"></param>
        /// <param name="lshort"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "wl_igbt_flow_start_test", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_flow_start_test(
            uint handle,
            byte[] items,
            byte[] krw,
            byte[] iges,
            byte[] vgeth,
            byte[] vcesat,
            byte[] ices,
            byte[] mpulse,
            byte[] spulse,
            byte[] lshort,
            byte[] vces,
            byte[] vf
        );

        /// <summary>
        /// 示波器配置状态获取（Card to PC）
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="oscilloscope_state"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "wl_igbt_get_oscilloscope_cfg_state", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_get_oscilloscope_cfg_state(uint handle, ref oscilloscope_state_t oscilloscope_state);


        /// <summary>
        /// 示波器配置完成（PC to Card）
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="is_success"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "wl_igbt_set_oscilloscope_cfg_state", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_set_oscilloscope_cfg_state(uint handle, byte is_success);

        /// <summary>
        /// 示波器采集完成（PC to Card）
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="is_success"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "wl_igbt_set_oscilloscope_sample_state", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_set_oscilloscope_sample_state(uint handle, byte is_success);


        /// <summary>
        /// 板卡发送脉冲完成
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="is_valid"></param>
        /// <param name="test_item"></param>
        /// <param name="test_state_para"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "wl_igbt_get_trigger_complete_state", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_get_trigger_complete_state(uint handle, ref byte is_valid, ref byte test_item, ref byte test_state_para);


        /// <summary>
        /// 序列当前测试项完成状态（Card to PC）
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="is_valid"></param>
        /// <param name="test_item"></param>
        /// <param name="is_success"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "wl_igbt_get_test_complete_state", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_get_test_complete_state(uint handle, ref byte is_valid, ref byte test_item, ref byte is_success);

        /// <summary>
        /// 开始执行下一个项测试
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>        
        [DllImportAttribute(DllPath, EntryPoint = "wl_igbt_flow_start_next", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_flow_start_next(uint handle);


        /// <summary>
        /// 获取当前测试状态
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="cbb_flow_state"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "wl_igbt_get_current_test_state", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_get_current_test_state(uint handle, ref cbb_flow_state_t cbb_flow_state);


        /// <summary>
        /// 上位机启动
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "wl_igbt_flow_pc_start", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_flow_pc_start(uint handle);


        /// <summary>
        /// 上位机停止
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "wl_igbt_flow_pc_stop", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_flow_pc_stop(uint handle);


        /// <summary>
        /// 上位机终止流程
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "wl_igbt_flow_exit", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_flow_exit(uint handle);


        /// <summary>
        /// 上位机复位流程
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImportAttribute(DllPath, EntryPoint = "wl_igbt_flow_reset", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_flow_reset(uint handle);

        #endregion

        #region wluart
        [DllImport(DllPath, EntryPoint = "cbb_wluart_set_para", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_wluart_set_para(uint handle, ref cbb_wluart_para_t wluart_para);

        [DllImport(DllPath, EntryPoint = "cbb_wluart_get_para", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_wluart_get_para(uint handle, ref cbb_wluart_para_t wluart_para);

        [DllImport(DllPath, EntryPoint = "cbb_wluart_frame_write", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_wluart_frame_write(uint handle, uint index, byte[] buff, uint length);

        [DllImport(DllPath, EntryPoint = "cbb_wluart_dev_en", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_wluart_dev_en(uint handle, byte[] dev_en, uint dev_num);

        [DllImport(DllPath, EntryPoint = "cbb_wluart_frame_read", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_wluart_frame_read(uint handle, ref uart_frame_t uart_frame, uint length, out uint real_length);

        [DllImport(DllPath, EntryPoint = "cbb_wluart_frame_read_length", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_wluart_frame_read_length(uint handle, out uint length);


        #endregion


        [DllImport(DllPath, EntryPoint = "wl_igbt_flow_trigger", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wl_igbt_flow_trigger(uint handle, byte type);


        [DllImportAttribute(DllPath, EntryPoint = "cbb_result_krw_read", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_result_krw_read(uint handle, IntPtr result_krw, uint length, IntPtr real_length);

        [DllImportAttribute(DllPath, EntryPoint = "cbb_result_iges_read", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_result_iges_read(uint handle, IntPtr result_iges, uint length, IntPtr real_length);

        [DllImportAttribute(DllPath, EntryPoint = "cbb_result_vgeth_read", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_result_vgeth_read(uint handle, IntPtr result_vgeth, uint length, IntPtr real_length);
       

        [DllImportAttribute(DllPath, EntryPoint = "cbb_result_ices_read", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_result_ices_read(uint handle,  IntPtr result_ices, uint length, IntPtr real_length);



        [DllImportAttribute(DllPath, EntryPoint = "cbb_result_vcesat_read", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_result_vcesat_read(uint handle, IntPtr result_vcesat, uint length, IntPtr real_length);

        [DllImportAttribute(DllPath, EntryPoint = "cbb_result_vf_read", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_result_vf_read(uint handle, IntPtr result_vf, uint length, IntPtr real_length);


        #region rt1000

        [DllImportAttribute(DllPath, EntryPoint = "cbb_rt1000_needle_bed", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_rt1000_needle_bed(uint handle, byte option, ref uint is_success);


        [DllImportAttribute(DllPath, EntryPoint = "cbb_rt1000_set_para", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_rt1000_set_para(uint handle, ref cbb_rt1000_para_t cbb_rt1000_para);


        [DllImportAttribute(DllPath, EntryPoint = "cbb_rt1000_get_para", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_rt1000_get_para(uint handle, ref cbb_rt1000_para_t cbb_rt1000_para);


        [DllImportAttribute(DllPath, EntryPoint = "cbb_rt1000_charge_ready", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_rt1000_charge_ready(uint handle, ref int is_success);

        [DllImportAttribute(DllPath, EntryPoint = "cbb_rt1000_charge_start", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_rt1000_charge_start(uint handle, ref IGBStructs.charge_state_t charge_state, uint overtime);

        [DllImportAttribute(DllPath, EntryPoint = "cbb_rt1000_discharge_start", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_rt1000_discharge_start(uint handle, ref charge_state_t discharge_state);

        [DllImportAttribute(DllPath, EntryPoint = "cbb_rt1000_write_do", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_rt1000_write_do(uint handle, uint id, uint on_off);

        [DllImportAttribute(DllPath, EntryPoint = "cbb_rt1000_read_rt1000_info_length", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_rt1000_read_rt1000_info_length(uint handle, ref uint length);

        [DllImportAttribute(DllPath, EntryPoint = "cbb_rt1000_clear", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_rt1000_clear(uint handle);


        [DllImportAttribute(DllPath, EntryPoint = "cbb_rt1000_read_rt1000_info", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbb_rt1000_read_rt1000_info(uint handle, IntPtr readBuffer, uint length, ref uint real_length);

        public static int cbb_rt1000_read_rt1000_info(uint handle, ref rt1000_info_t rt1000_info, uint length, ref uint real_length)
        {
            int dataSize = Marshal.SizeOf(rt1000_info);
            IntPtr buffer = Marshal.AllocHGlobal((int)(dataSize * length));
            try
            {
                uint readLength = 0;
                int errorCode = cbb_rt1000_read_rt1000_info(handle, buffer, length, ref readLength);
                if (readLength > 0)
                {
                    rt1000_info = Marshal.PtrToStructure<rt1000_info_t>(buffer + (int)((readLength - 1) * dataSize));
                }
                return errorCode;
            }
            finally
            {
                Marshal.Release(buffer);
            }
        }

        #endregion

     
    }
}
