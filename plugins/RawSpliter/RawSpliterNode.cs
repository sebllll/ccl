#region usings
using System;
using System.IO;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "Spliter", Category = "Raw", Help = "Basic raw template which copies up to count bytes from the input to the output", Tags = "")]
	#endregion PluginInfo
	public class RawSpliterNode : IPluginEvaluate, IPartImportsSatisfiedNotification
	{
		#region fields & pins
		[Input("Input")]
		public ISpread<Stream> FStreamIn;
		
		[Input("Separator")]
		public ISpread<string> FSeparator;

		[Output("Output")]
		public ISpread<Stream> FStreamOut;

		//when dealing with byte streams (what we call Raw in the GUI) it's always
		//good to have a byte buffer around. we'll use it when copying the data.
		readonly byte[] FBuffer = new byte[1024];
		#endregion fields & pins

		//called when all inputs and outputs defined above are assigned from the host
		public void OnImportsSatisfied()
		{
			//start with an empty stream output
			FStreamOut.SliceCount = 0;
		}

		
		byte[] SeparateAndGetLast(byte[] source, byte[] separator)
		{
		  for (var i = 0; i < source.Length; ++i)
		  {
		     if(Equals(source, separator, i))
		     {
		       var index = i + separator.Length;
		       var part = new byte[source.Length - index];
		       Array.Copy(source, index, part, 0, part.Length);
		       return part;
		     }
		  }
		  throw new Exception("not found");
		}
		
		public static byte[][] Separate(byte[] source, byte[] separator)
		{
		    var Parts = new List<byte[]>();
		    var Index = 0;
		    byte[] Part;
		    for (var I = 0; I < source.Length; ++I)
		    {
		        if (Equals(source, separator, I))
		        {
		            Part = new byte[I - Index];
		            Array.Copy(source, Index, Part, 0, Part.Length);
		            Parts.Add(Part);
		            Index = I + separator.Length;
		            I += separator.Length - 1;
		        }
		    }
		    Part = new byte[source.Length - Index];
		    Array.Copy(source, Index, Part, 0, Part.Length);
		    Parts.Add(Part);
		    return Parts.ToArray();
		}
		
		bool Equals(byte[] source, byte[] separator, int index)
		{
		  for (int i = 0; i < separator.Length; ++i)
		    if (index + i >= source.Length || source[index + i] != separator[i])
		      return false;
		  return true;
		}


		
		//called when data for any output pin is requested
		public void Evaluate(int spreadMax)
		{
			//ResizeAndDispose will adjust the spread length and thereby call
			//the given constructor function for new slices and Dispose on old
			//slices.
			string strseperate = "EVILSEPERATOREVIL";
    		System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
    		byte[] seperator = encoding.GetBytes(FSeparator[0]);
    		//Split code should go here

			
			FStreamOut.ResizeAndDispose(spreadMax, () => new MemoryStream());
			for (int i = 0; i < spreadMax; i++) {
				//get the input stream
				var inputStream = FStreamIn[i];
				//get the output stream (this works because of ResizeAndDispose above)
				var outputStream = FStreamOut[i];
				//get the number of bytes we should copy (avoid negative values)
				var slice = 0;
				
				
			    var Index = 0;
			    byte[] Part;
			    for (var I = 0; I < inputStream.Length; ++I)
			    {
			        if (Equals(inputStream, separator, I))
			        {
			            Part = new byte[I - Index];
			            Array.Copy(inputStream, Index, Part, 0, Part.Length);
			            FOutput.Add(Part);
			            Index = I + separator.Length;
			            I += separator.Length - 1;
			        }
			    }
			    Part = new byte[inputStream.Length - Index];
			    Array.Copy(inputStream, Index, Part, 0, Part.Length);
			    FOutput.Add(Part);
			    
				/*
				
				
				numBytesToCopy = outputStream.Length;
				//look for #bundle
				while (numBytesToCopy > 0) {
					
					var chunkSize = 1;
					numBytesRead = FStreamIn[i].Read(FBuffer,0,1);
					if(FBuffer[0] == '#') 
					{
						slice++;
					}
					if (numBytesRead == 0)
						break;
					numBytesToCopy--;
				}
				
				var count = Math.Max(FCountIn[i], 0);
				//see how many bytes should be copied from the input stream
				var numBytesToCopy = Math.Min(inputStream.Length, count);

				//reset the positions of the streams
				inputStream.Position = 0;
				outputStream.Position = 0;

				//set the length of the output stream
				outputStream.SetLength(numBytesToCopy);

				//finally copy the data
				while (numBytesToCopy > 0) {
					//make sure we don't read more than we need or more than
					//our byte buffer can hold
					var chunkSize = (int)Math.Min(numBytesToCopy, FBuffer.Length);
					//the stream's read method returns how many bytes have actually
					//been read into the buffer
					var numBytesRead = inputStream.Read(FBuffer, 0, chunkSize);
					//in case nothing has been read we need to leave the loop
					//as we requested more than there was available
					if (numBytesRead == 0)
						break;
					//write the number of bytes read to the output stream
					outputStream.Write(FBuffer, 0, numBytesRead);
					//decrease the total amount of bytes we still need to read
					numBytesToCopy -= numBytesRead;
				}
			}
*/
			//this will force the changed flag of the output pin to be set
			FStreamOut.Flush(true);
		}
	}
}
