using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Pistol, Shotgun은 없음
// LMG, AR, AK는 연사 속도 변경, 반동 변경 --> Action, Recoil 변화
// SMG는 발사 방식 변경, 연사 속도 변경, 반동 변경 ---> Action, Recoil, Result 변화
// Sniper는 탄 퍼짐 변화, 공격 속도 변화 ---> Action, Recoil 변화

abstract public class VariationGun : Gun
{
    public enum Conditon
    {
        Both, // 모든 조건에서 사용됨
        ZoomIn, // 줌 적용 시에만 사용됨
        ZoomOut // 줌 해제 시에만 사용됨
    }

    protected Dictionary<Tuple<EventType, Conditon>, EventStrategy> _eventStorage = new();
    protected Dictionary<Tuple<EventType, Conditon>, ActionStrategy> _actionStorage = new();
    protected Dictionary<Tuple<EventType, Conditon>, BaseRecoilStrategy> _recoilStorage = new();

    public override void OnRooting(WeaponBlackboard blackboard)
    {
        base.OnRooting(blackboard);
        foreach (var eventStrategy in _eventStorage) eventStrategy.Value.LinkEvent(blackboard);
        foreach (var actionStrategy in _actionStorage) actionStrategy.Value.LinkEvent(blackboard);
        foreach (var recoilStrategy in _recoilStorage) recoilStrategy.Value.LinkEvent(blackboard);
    }

    public override void OnDrop(WeaponBlackboard blackboard)
    {
        base.OnDrop(blackboard);
        foreach (var eventStrategy in _eventStorage) eventStrategy.Value.UnlinkEvent(blackboard);
        foreach (var actionStrategy in _actionStorage) actionStrategy.Value.UnlinkEvent(blackboard);
        foreach (var recoilStrategy in _recoilStorage) recoilStrategy.Value.UnlinkEvent(blackboard);
    }

    // state 패턴으로 바꿔주기
    protected void OnZoomRequested(bool nowZoom)
    {
        if (nowZoom) OnZoomIn();
        else OnZoomOut();
    }

    protected virtual void OnZoomIn() 
    {
        Tuple<EventType, Conditon> zoomInKey = new(EventType.Main, Conditon.ZoomIn);
        if (_eventStorage.ContainsKey(zoomInKey)) _eventStrategies[EventType.Main] = _eventStorage[zoomInKey];
        if (_actionStorage.ContainsKey(zoomInKey)) _actionStrategies[EventType.Main] = _actionStorage[zoomInKey];
        if (_recoilStorage.ContainsKey(zoomInKey)) _recoilStrategies[EventType.Main] = _recoilStorage[zoomInKey];
    }

    protected virtual void OnZoomOut() 
    {
        Tuple<EventType, Conditon> zoomOutKey = new(EventType.Main, Conditon.ZoomOut);
        if (_eventStorage.ContainsKey(zoomOutKey)) _eventStrategies[EventType.Main] = _eventStorage[zoomOutKey];
        if (_actionStorage.ContainsKey(zoomOutKey)) _actionStrategies[EventType.Main] = _actionStorage[zoomOutKey];
        if (_recoilStorage.ContainsKey(zoomOutKey)) _recoilStrategies[EventType.Main] = _recoilStorage[zoomOutKey];
    }

    public override void MatchStrategy()
    {
        Tuple<EventType, Conditon> mainBothKey = new(EventType.Main, Conditon.Both);
        Tuple<EventType, Conditon> subBothKey = new(EventType.Sub, Conditon.Both);

        Tuple<EventType, Conditon> mainZoomOutKey = new(EventType.Main, Conditon.ZoomOut);
        Tuple<EventType, Conditon> subZoomOutKey = new(EventType.Sub, Conditon.ZoomOut);


        if (_eventStorage.ContainsKey(mainBothKey)) _eventStrategies[EventType.Main] = _eventStorage[mainBothKey];
        else _eventStrategies[EventType.Main] = _eventStorage[mainZoomOutKey];

        if (_eventStorage.ContainsKey(subBothKey)) _eventStrategies[EventType.Sub] = _eventStorage[subBothKey];
        else _eventStrategies[EventType.Sub] = _eventStorage[subZoomOutKey];


        if (_actionStorage.ContainsKey(mainBothKey)) _actionStrategies[EventType.Main] = _actionStorage[mainBothKey];
        else _actionStrategies[EventType.Main] = _actionStorage[mainZoomOutKey];

        if (_actionStorage.ContainsKey(subBothKey)) _actionStrategies[EventType.Sub] = _actionStorage[subBothKey];
        else _actionStrategies[EventType.Sub] = _actionStorage[subZoomOutKey];


        if (_recoilStorage.ContainsKey(mainBothKey)) _recoilStrategies[EventType.Main] = _recoilStorage[mainBothKey];
        else _recoilStrategies[EventType.Main] = _recoilStorage[mainZoomOutKey];

        if (_recoilStorage.ContainsKey(subBothKey)) _recoilStrategies[EventType.Sub] = _recoilStorage[subBothKey];
        else _recoilStrategies[EventType.Sub] = _recoilStorage[subZoomOutKey];
    }
}