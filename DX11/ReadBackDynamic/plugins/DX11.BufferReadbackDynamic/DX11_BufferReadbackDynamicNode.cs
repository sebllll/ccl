using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.NonGeneric;

using SlimDX;

using FeralTic.DX11.Resources;
using FeralTic.DX11;
using SlimDX.Direct3D11;
using VVVV.Hosting.IO.Pointers;
using System.Threading.Tasks;

namespace VVVV.DX11.Nodes
{
	[PluginInfo(Name = "ReadBackDynamic", Category = "DX11.Buffer", Version = "Vector3", Author = "vux", AutoEvaluate = false)]
	public unsafe class ReadBackBufferColor3Node : IPluginEvaluate, IDX11ResourceDataRetriever
	{
		[Input("Input")]
		protected Pin<DX11Resource<IDX11RWStructureBuffer>> FInput;
		
		[Input("ElementCount", DefaultValue = 1, IsSingle = true)]
		protected ISpread<int> FElementCount;
		
		[Input("Enabled", DefaultValue = 1, IsSingle = true)]
		protected ISpread<bool> FInEnabled;
		
		[Output("xyz")]
		protected ValueOutput FOutput;
		
		[Output("radius")]
		protected ValueOutput FOutput2;
		
		[Output("collision")]
		protected ValueOutput FOutput3;
		
		[Output("event")]
		protected ValueOutput FOutput4;
		
		[Import()]
		protected IPluginHost FHost;
		
		private DX11StagingStructuredBuffer staging;
		
		public DX11RenderContext AssignedContext
		{
			get;
			set;
		}
		
		public event DX11RenderRequestDelegate RenderRequest;
		
		
		#region IPluginEvaluate Members
		public void Evaluate(int SpreadMax)
		{
			if (this.FInput.PluginIO.IsConnected && this.FInEnabled[0])
			{
				if (this.RenderRequest != null) { this.RenderRequest(this, this.FHost); }
				
				if (this.AssignedContext == null)
				{
					this.FOutput.Length = 0;
					this.FOutput2.Length = 0;
					this.FOutput3.Length = 0;
					this.FOutput4.Length = 0;
					return;
				}
				
				IDX11RWStructureBuffer b = this.FInput[0][this.AssignedContext];
				if (b != null)
				{
					if (this.staging != null && this.staging.ElementCount != b.ElementCount) { this.staging.Dispose(); this.staging = null; }
					
					int FinalElementCount = Math.Min(FElementCount[0], b.ElementCount);
					
					if (this.staging == null)
					{
//						staging = new DX11StagingStructuredBuffer(this.AssignedContext.Device, b.ElementCount, 24);
						staging = new DX11StagingStructuredBuffer(this.AssignedContext.Device, FinalElementCount, 24);
					}
					
					this.AssignedContext.CurrentDeviceContext.CopyResource(b.Buffer, staging.Buffer);
					
						
					
					this.FOutput.Length = FinalElementCount*3; //float3
					this.FOutput2.Length = FinalElementCount;	//float
					this.FOutput3.Length = FinalElementCount;	//float
					this.FOutput4.Length = FinalElementCount;	//uint
					
					DataStream ds = staging.MapForRead(this.AssignedContext.CurrentDeviceContext);
					
					double* ptr1 = this.FOutput.Data;
					double* ptr2 = this.FOutput2.Data;
					double* ptr3 = this.FOutput3.Data;
					double* ptr4 = this.FOutput4.Data;
										
									
					
//					for (int i = 0; i < b.ElementCount; i++)
					for (int i = 0; i < FinalElementCount; i++)
					{
						
//						Vector3 v = ds.Read<Vector3>();
//						ptr1[i*3] = v.X;
//						ptr1[i * 3 + 1] = v.Y;
//						ptr1[i * 3 + 2] = v.Z;
						
						Vector6 v = ds.Read<Vector6>();
						
						ptr1[i*3] = v.X;
						ptr1[i * 3 + 1] = v.Y;
						ptr1[i * 3 + 2] = v.Z;
						
						ptr2[i * 4]  = v.R;
						
						ptr3[i * 5] = v.G;
						
						ptr4[i * 6] = v.B;
						
						
					}
					
					staging.UnMap(this.AssignedContext.CurrentDeviceContext);
				}
				else
				{
					this.FOutput.Length = 0;
					this.FOutput2.Length = 0;
					this.FOutput3.Length = 0;
					this.FOutput4.Length = 0;
				}
			}
			else
			{
				this.FOutput.Length = 0;
				this.FOutput2.Length = 0;
				this.FOutput3.Length = 0;
				this.FOutput4.Length = 0;
			}
		}
		#endregion
		
		struct Vector6
		{
			private float xval;
			public float X
			{
				get
				{
					return xval;
				}
				set
				{
					xval = value;
				}
			}
			
			private float yval;
			public float Y
			{
				get
				{
					return yval;
				}
				set
				{
					yval = value;
				}
			}
			
			private float zval;
			public float Z
			{
				get
				{
					return zval;
				}
				set
				{
					zval = value;
				}
			}
			
			private float rval;
			public float R
			{
				get
				{
					return rval;
				}
				set
				{
					rval = value;
				}
			}
			
			private float gval;
			public float G
			{
				get
				{
					return gval;
				}
				set
				{
					gval = value;
				}
			}
			
			private uint bval;
			public uint B
			{
				get
				{
					return bval;
				}
				set
				{
					bval = value;
				}
			}

		}
		
	}
}