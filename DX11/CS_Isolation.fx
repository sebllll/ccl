
//This is the buffer from the renderer
//Renderer automatically assigns BACKBUFFER semantic
RWStructuredBuffer<float> rwbuffer : BACKBUFFER;

//Texture we want to read from
Texture2D tex <string uiname="Texture";>;

Texture2D texPlayer <string uiname="Player Texture";>;

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
	if (i.x == 0)
	{
		rwbuffer[0] = 0.0f;
		rwbuffer[1] = 0.0f;
		//rwbuffer[2] = 0.0f;
	}
	
	float playerLookup = texPlayer.SampleLevel(mySampler,uv[i.x],0).b;
	
	if (playerLookup > 0.06)
	{
		rwbuffer[0] += 1.0f;
		
		float2 lookup = tex.SampleLevel(mySampler,uv[i.x],0).rg;
		rwbuffer[1] += abs(lookup.r / 2) + abs(lookup.g / 2);
	}
	
	//rwbuffer[2] += 1.0f;
}

technique11 Process
{
	pass P0
	{
		SetComputeShader( CompileShader( cs_4_0, CS() ) );
	}
}







