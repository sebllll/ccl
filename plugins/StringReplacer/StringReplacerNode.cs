#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "Replacer", Category = "String", Help = "Basic template with one string in/out", Tags = "")]
	#endregion PluginInfo
	public class StringReplacerNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultString = "hello %test%", Order = 0)]
		public ISpread<string> FInput;
		
		[Input("Template", DefaultString = "%test%", Order = 1)]
		public ISpread<string> FTemplate;
		
		[Input("Value", DefaultString = "vvvv", Order = 2)]
		public ISpread<ISpread<string>> FValue;

		[Output("Output")]
		public ISpread<string> FOutput;

		[Import()]
		public ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FOutput.SliceCount = FValue.SliceCount;
			
			//if(FValue.IsChanged) 
			for (int i = 0; i < FValue.SliceCount; i++) {
				FOutput[i] = FInput[0];
				for (int t = 0; t< FTemplate.SliceCount; t++)
				{
				
					FOutput[i] = FOutput[i].Replace(FTemplate[t], FValue[i][t]);
					
				}
			} 
			//FLogger.Log(LogType.Debug, cnt.ToString());
		}
	}
}
