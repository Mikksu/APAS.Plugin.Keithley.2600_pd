namespace APAS.Plugin.KEYTHLEY.SMU2600
{
    public enum VoltUnitEnum
    {
       V = 1000000000,
       mV = 1000000,
       uV = 1000,
       nV = 1,
    }

    public enum CurrentUnitEnum
    {
        A = 1000000000,
        mA = 1000000,
        uA = 1000,
        nA = 1
    }

    public enum SourceModeEnum
    {
        /// <summary>
        /// 电流源模式
        /// </summary>
        ISource,

        /// <summary>
        /// 电压源模式
        /// </summary>
        VSource
    }
}
