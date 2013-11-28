
//This is the buffer from the renderer
//Renderer automatically assigns BACKBUFFER semantic
RWStructuredBuffer<float> rwbuffer : BACKBUFFER;

//Texture we want to read from
Texture2D tex <string uiname="Texture";>;

//Buffer containing uvs for sampling
StructuredBuffer<float2> uv <string uiname="UV Buffer";>;

int Binsize = 128;

SamplerState mySampler : IMMUTABLE
{
    Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Clamp;
    AddressV = Clamp;
};


[numthreads(1, 1, 1)]
void CS( uint3 i : SV_DispatchThreadID)
{ 
	if (i.x == 0)
	{
		for ( int b = 0; b < Binsize-1; b++)
		{
			rwbuffer[b] = 0.5f;
		}
	}
	
	//rwbuffer[0] = 0.75f;
	
	float2 lookup = tex.SampleLevel(mySampler,uv[i.x],0).rg;
	
	int slot = abs(lookup.r * Binsize);
	
	//rwbuffer[slot] += 1.0f;

}

technique11 Process
{
	pass P0
	{
		SetComputeShader( CompileShader( cs_4_0, CS() ) );
	}
}







