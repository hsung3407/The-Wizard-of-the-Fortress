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

        //효과 적용과 해제
        public abstract void Execute();
        public abstract void Release();

        //해당 효과의 적용이 더 이상 유효하지 않게 되었는지 확인 (예시: 적용 시간 초과)
        public abstract bool IsExpired();
    }
}