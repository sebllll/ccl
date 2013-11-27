
//This is the buffer from the renderer
//Renderer automatically assigns BACKBUFFER semantic
RWStructuredBuffer<float> rwbuffer : BACKBUFFER;


//Texture we want to read from
Texture2D tex <string uiname="Texture";>;

//Buffer containing uvs for sampling
StructuredBuffer<float2> uv <string uiname="UV Buffer";>;

SamplerState mySampler : IMMUTABLE
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Clamp;
	AddressV = Clamp;
};




[numthreads(1, 1, 1)]
void CS( uint3 i : SV_DispatchThreadID)
{
	if (i.x == 0) { // initialize buffers for each frame at the first pixel only
		rwbuffer[0] = 10;
		rwbuffer[1] = -10;
	}
	
	float lookup = tex.SampleLevel(mySampler,uv[i.x],0).r;
	
	if( lookup != 0)
	{
		
		if ( lookup < rwbuffer[0]) rwbuffer[0] = lookup;
		
		if (lookup > rwbuffer[1]) rwbuffer[1] = lookup;
		
	}
}

technique11 Process
{
	pass P0
	{
		SetComputeShader( CompileShader( cs_5_0, CS() ) );
	}
}







