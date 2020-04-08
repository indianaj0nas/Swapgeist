//shadow rendering input and output
#ifndef MK_TOON_SHADOWCASTER
	#define MK_TOON_SHADOWCASTER

	/////////////////////////////////////////////////////////////////////////////////////////////
	// VERTEX SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	VertexOutputShadowCaster vertShadowCaster (VertexInputShadowCaster v)
	{
		VertexOutputShadowCaster o;
		UNITY_INITIALIZE_OUTPUT(VertexOutputShadowCaster, o);
		
		#ifdef SHADOWS_CUBE //point light shadows
			o.pos = UnityObjectToClipPos(v.vertex);
			o.sv = mul(unity_ObjectToWorld, v.vertex).xyz - _LightPositionRange.xyz;
		#else //other shadows
			//pos with unity macros
			o.pos = UnityClipSpaceShadowCasterPos(v.vertex.xyz, v.normal);
			o.pos = UnityApplyLinearShadowBias(o.pos);
		#endif
		return o;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////
	// FRAGMENT SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	half4 fragShadowCaster 
		(
			VertexOutputShadowCaster o
		) : SV_Target
	{	
		#ifdef SHADOWS_CUBE
			return UnityEncodeCubeShadowDepth ((length(o.sv) + unity_LightShadowBias.x) * _LightPositionRange.w);
		#else
			return 0;
		#endif
	}			
#endif