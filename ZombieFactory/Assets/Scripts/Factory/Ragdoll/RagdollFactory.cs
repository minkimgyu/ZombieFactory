using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollCreater
{
    protected Ragdoll _ragdoll;

    public RagdollCreater(Ragdoll prefab)
    {
        _ragdoll = prefab;
    }

    public Ragdoll Create(Vector3 pos, Quaternion rotation)
    {
        Ragdoll ragdoll = Object.Instantiate(_ragdoll, pos, rotation);
        ragdoll.transform.position = pos;
        ragdoll.transform.rotation = rotation;

        return ragdoll;
    }
}

public class RagdollFactory : BaseFactory
{
    Dictionary<BaseLife.Name, RagdollCreater> _ragdollCreaters;

    public RagdollFactory(AddressableHandler addressableHandler)
    {
        _ragdollCreaters = new Dictionary<BaseLife.Name, RagdollCreater>();

        _ragdollCreaters[BaseLife.Name.PoliceZombie] = new RagdollCreater(addressableHandler.RagdollPrefabs[BaseLife.Name.PoliceZombie]);
        _ragdollCreaters[BaseLife.Name.WitchZombie] = new RagdollCreater(addressableHandler.RagdollPrefabs[BaseLife.Name.WitchZombie]);
        _ragdollCreaters[BaseLife.Name.MaskZombie] = new RagdollCreater(addressableHandler.RagdollPrefabs[BaseLife.Name.MaskZombie]);
        _ragdollCreaters[BaseLife.Name.Warden] = new RagdollCreater(addressableHandler.RagdollPrefabs[BaseLife.Name.Warden]);
        _ragdollCreaters[BaseLife.Name.Rook] = new RagdollCreater(addressableHandler.RagdollPrefabs[BaseLife.Name.Rook]);
    }

    public override Ragdoll Create(BaseLife.Name name, Vector3 pos, Quaternion rotation)
    {
        return _ragdollCreaters[name].Create(pos, rotation);
    }
}