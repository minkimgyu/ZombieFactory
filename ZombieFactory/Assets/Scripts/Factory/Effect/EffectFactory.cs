using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectCreater : Pool
{
    public EffectCreater(BaseEffect prefab, int startCount, Transform parent) : base(prefab, startCount, parent)
    {
    }

    public BaseEffect Create()
    {
        BaseEffect effect = (BaseEffect)_pool.Get();
        effect.Initialize();
        return effect;
    }
}

public class EffectFactory : BaseFactory
{
    Dictionary<BaseEffect.Name, EffectCreater> _effectCreaters;

    public EffectFactory(AddressableHandler addressableHandler, Transform parent)
    {
        _effectCreaters = new Dictionary<BaseEffect.Name, EffectCreater>();

        // 여기서 추가
        _effectCreaters[BaseEffect.Name.PenetrateBulletHole] = new EffectCreater(addressableHandler.EffectPrefabs[BaseEffect.Name.PenetrateBulletHole], 10, parent);
        _effectCreaters[BaseEffect.Name.NonPenetrateBulletHole] = new EffectCreater(addressableHandler.EffectPrefabs[BaseEffect.Name.NonPenetrateBulletHole], 10, parent);
        _effectCreaters[BaseEffect.Name.WallFragmentation] = new EffectCreater(addressableHandler.EffectPrefabs[BaseEffect.Name.WallFragmentation], 10, parent);
        _effectCreaters[BaseEffect.Name.KnifeMark] = new EffectCreater(addressableHandler.EffectPrefabs[BaseEffect.Name.KnifeMark], 3, parent);
        _effectCreaters[BaseEffect.Name.DamageTxt] = new EffectCreater(addressableHandler.EffectPrefabs[BaseEffect.Name.DamageTxt], 10, parent);
        _effectCreaters[BaseEffect.Name.Explosion] = new EffectCreater(addressableHandler.EffectPrefabs[BaseEffect.Name.Explosion], 0, parent);
        _effectCreaters[BaseEffect.Name.TrajectoryLine] = new EffectCreater(addressableHandler.EffectPrefabs[BaseEffect.Name.TrajectoryLine], 10, parent);
    }

    public override BaseEffect Create(BaseEffect.Name name)
    {
        return _effectCreaters[name].Create();
    }
}
