
//This is the buffer from the renderer
//Renderer automatically assigns BACKBUFFER semantic
RWStructuredBuffer<float> rwbuffer : BACKBUFFER;

//Texture we want to read from
Texture2D tex <string uiname="Texture";>;
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
	
	float lookupTex = tex.SampleLevel(mySampler,uv[i.x].r,0);
	float lookupSmoothTex = smoothedTex.SampleLevel(mySampler,uv[i.x].r,0);
	rwbuffer[i.x] = lookupTex - lookupSmoothTex;
	
}

technique11 Process
{
	pass P0
	{
		SetComputeShader( CompileShader( cs_5_0, CS() ) );
	}
}







