using System;

namespace Ingame.Player.Effect
{
    public abstract class EffectCommand
    {
        public readonly EffectID EffectID;
        public readonly Object Obj;

        protected EffectCommand(EffectID effectID, Object obj)
        {
            EffectID = effectID;
            Obj = obj;
        }

        public abstract void Execute();
        public abstract void Release();

        public abstract bool IsExpired();
    }
}