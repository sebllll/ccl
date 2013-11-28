
//This is the buffer from the renderer
//Renderer automatically assigns BACKBUFFER semantic
RWStructuredBuffer<float> rwbuffer : BACKBUFFER;

//Texture we want to read from
Texture2D tex <string uiname="Texture";>;

Texture2D texPlayer <string uiname="Player Texture";>;

//Buffer containing uvs for sampling
StructuredBuffer<float2> uv <string uiname="UV Buffer";>;

int Binsize = 128;

int Divisor = 100;

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
		for ( int b = 0; b < (Binsize*2); b++)
		{
			rwbuffer[b] = 0.0f;
		}
	}
	
	float2 lookup = tex.SampleLevel(mySampler,uv[i.x],0).rg;
	
	float playerLookup = texPlayer.SampleLevel(mySampler,uv[i.x],0).b;
	
	if (playerLookup > 0.06)
	{
		
		uint slotR = abs(lookup.r * Binsize );
		uint slotG = abs(lookup.g * Binsize)  + Binsize-1;
		
		rwbuffer[slotR] += lookup.r / Divisor;
		rwbuffer[slotG] += lookup.g / Divisor;

//		rwbuffer[slotR] += sign(lookup.r) * 1 / Divisor;
//		rwbuffer[slotG] += sign(lookup.g) * 1 / Divisor;
		
	}
}

technique11 Process
{
	pass P0
	{
		SetComputeShader( CompileShader( cs_4_0, CS() ) );
	}
}







