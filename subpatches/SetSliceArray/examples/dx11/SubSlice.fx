//@author: vux
//@help: View a specific slice for texture array 
//@tags: 
//@credits: 

Texture2DArray texture2d;

SamplerState linearSampler : IMMUTABLE
{
    Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Clamp;
    AddressV = Clamp;
};

 
cbuffer cbBuffer : register( b0 )
{
	float4x4 tWVP : WORLDVIEWPROJECTION;
};

struct vsInput
{
	float4 pos : POSITION;
	float4 uv : TEXCOORD0;
};

struct psInput
{
    float4 pos : SV_POSITION;
    float4 uv: TEXCOORD0;
};

psInput VS(vsInput input)
{
    psInput output;
    output.pos  = mul(input.pos,tWVP);
    output.uv = input.uv;
    return output;
}


float4 PS(psInput input): SV_Target
{
    float4 col = texture2d.Sample(linearSampler,float3(input.uv.xy,0));
	float4 col2 = texture2d.Sample(linearSampler,float3(input.uv.xy,1));
	return float4(abs(col.rgb-col2.rgb),1.0f);
}

technique10 Render
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetPixelShader( CompileShader( ps_4_0, PS() ) );
	}
}




