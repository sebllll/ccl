using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FeralTic.DX11;
using System.Reflection;

namespace MrVux.Lib
{
    public class ShaderUtils
    {
        public static DX11ShaderInstance GetShader(DX11RenderContext context, string shadername)
        {
            DX11Effect effect = DX11Effect.FromResource(Assembly.GetExecutingAssembly(), "VVVV.Nodes.DX11SetSlice.effects." + shadername + ".fx");
            return new DX11ShaderInstance(context, effect);
        }
    }
}
