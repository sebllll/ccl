
//This is the buffer from the renderer
//Renderer automatically assigns BACKBUFFER semantic



RWStructuredBuffer<float> rwbuffer : BACKBUFFER;


//Texture we want to read from
Texture2D tex <string uiname="Texture";>;
Texture2D delayedTex <string uiname="DelayedTex";>;
Texture2D smoothedTex <string uiname="SmoothedTexture";>;

//Buffer containing uvs for sampling
StructuredBuffer<float2> uv <string uiname="UV Buffer";>;

SamplerState mySampler : IMMUTABLE
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Clamp;
	AddressV = Clamp;
};


[numthreads(64, 1, 1)]
void CS( uint3 i : SV_DispatchThreadID)
{
	//Read color and writed to buffer
	//float framedifference
	
	float lookupTex = tex.SampleLevel(mySampler,uv[i.x],0).r + tex.SampleLevel(mySampler,uv[i.x],0).g ;
	float lookDelayedTex = delayedTex.SampleLevel(mySampler,uv[i.x],0).r + delayedTex.SampleLevel(mySampler,uv[i.x],0).g;
	float lookupSmoothTex = smoothedTex.SampleLevel(mySampler,uv[i.x],0).r + smoothedTex.SampleLevel(mySampler,uv[i.x],0).g;
	rwbuffer[i.x] = abs(lookupTex - lookDelayedTex);
	
	//rwbuffer[i.x].Smoothness = abs(lookupTex - lookupSmoothTex);
	

	
	
	//rwbuffer[i.x].Speed = abs(lookupTex - lookDelayedTex);
	
}

technique11 Process
{
	pass P0
	{
		SetComputeShader( CompileShader( cs_4_0, CS() ) );
	}
}







