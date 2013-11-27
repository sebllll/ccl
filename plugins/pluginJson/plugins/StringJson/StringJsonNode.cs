#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using VVVV.Core.Logging;
using System.Collections.Generic;

#endregion usings

namespace VVVV.Nodes
{

	[PluginInfo(Name = "JsonParser", Category = "JSON", Help = "parse json string", Tags = "")]
	public class JsonParser : IPluginEvaluate
	{
		#region fields & pins
		[Input("JSON", DefaultString = "hello")]
		IDiffSpread<string> FInput;

		

		
	
		
		
	    [Output("Output json")]
		ISpread<JObject> FJOutput;
		
		
	
		[Import()]
		ILogger FLogger;
		#endregion fields & pins
		//called when data for any output pin is requested
		
        
		
		public void Evaluate(int SpreadMax)
		{
		 
	
	if(FInput.IsChanged )
	
	       
    
           FJOutput[0] = JObject.Parse(FInput[0]);
	       FJOutput.SliceCount = 1;
     
}

	}


	
	
	
    [PluginInfo(Name = "SelectToken", Category = "JSON", Help = "select json token", Tags = "")]
	public class SelectToken : IPluginEvaluate
	{
		#region fields & pins
		[Input("JObject")]
		ISpread<JObject> FInput;

	    [Input("path")]
		ISpread<string> FInputp;
		
	
		[Output("Output")]
		ISpread<string> FOutput;

		
		
		
	
		[Import()]
		ILogger FLogger;
		#endregion fields & pins
		//called when data for any output pin is requested
		
        
		
		public void Evaluate(int SpreadMax)
		{
		 
		  FOutput.SliceCount = SpreadMax;
		
		 
		  if( FInput[0]!=null){
		
		for (int i = 0; i < FInputp.SliceCount; i++)
          	
          	{
          		
           if( FInput[0].SelectToken(FInputp[i])!= null && FInput[0]!=null){
          	
           FOutput[i]=  FInput[0].SelectToken(FInputp[i]).ToString();
          
          	}else{FOutput[i]= "";}
           
           } 
		  
		
	
}

	}
}
	
	
	
	
	
	
    [PluginInfo(Name = "JsonArray", Category = "JSON", Help = "list json array content", Tags = "")]
	public class JsonArray : IPluginEvaluate
	{
		#region fields & pins
		[Input("Jobject")]
		ISpread<JObject> FInput;

	    [Input("path")]
		ISpread<string> FInputp;
		
	   [Input("key")]
		ISpread<string> FInputk;

		[Output("Output")]
		ISpread<string> FOutput;

        [Output("count")]
		ISpread<int> FcOutput;
		
	   // [Output("Output json")]
		//ISpread<Vson> FJOutput;
		
		
	
		[Import()]
		ILogger FLogger;
		#endregion fields & pins
		//called when data for any output pin is requested
		
        
		
		public void Evaluate(int SpreadMax)
		{
		 
           int i = 0;
			FcOutput.SliceCount = 1;
		   if( FInput[0]!=null){
           if( FInput[0].SelectToken(FInputp[i])!= null  ){
          		
          	var results  = FInput[0].SelectToken(FInputp[i]);
          	foreach (JToken child in results.Children())
            {
            if( child.SelectToken(FInputk[0])!= null ){
	        FOutput[i] = child.SelectToken(FInputk[0]).ToString();
	      } else { FOutput[i] = "";} 
            	
            	
            
              i++;
          
          	
           }

		  FOutput.SliceCount = i;
		  FcOutput[0] = i;
	}}else{
    FOutput.SliceCount = 1;
	FcOutput[0] = 0;	
	FOutput[i]= "";}
}
	
	
	
	}
	
	
	
	
}
