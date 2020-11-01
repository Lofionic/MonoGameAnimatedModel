using System.IO;
using System.Reflection;

namespace Lofionic.AnimatedModel.Effects {

    internal class EffectResource {

#if DIRECTX
        internal static EffectResource AnimatedModel = new EffectResource("AnimatedModelLib.Effects.AnimatedModelShader.dx11.mgfxo");
#else
        internal static EffectResource AnimatedModel = new EffectResource("AnimatedModelLib.Effects.AnimatedModelShader.ogl.mgfxo");
#endif

        private readonly object _locker = new object();
        private readonly string _name;
        private volatile byte[] _bytecode;

        private EffectResource(string name) {
            _name = name;
        }

        public byte[] Bytecode {
            get {
                if (_bytecode == null) {
                    lock (_locker) {
                        if (_bytecode != null) {
                            return _bytecode;
                        }

                        Stream stream = typeof(EffectResource).GetTypeInfo().Assembly.GetManifestResourceStream(_name);
                        using (MemoryStream ms = new MemoryStream()) {
                            stream.CopyTo(ms);
                            _bytecode = ms.ToArray();
                        }
                    }
                }
                return _bytecode;
            }
        }
    }
}
