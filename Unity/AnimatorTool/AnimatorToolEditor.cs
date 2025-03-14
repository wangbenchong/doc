#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.Collections;

public class AnimatorToolEditor : EditorWindow
{
    private static AnimatorToolEditor editor = null;
    [MenuItem("Tools/Animator Tool")]
    static void Init()
    {
        editor = GetWindow<AnimatorToolEditor>();
        editor.titleContent = new GUIContent("Animator Tool");
    }
    void OnEnable()
    {
    }
    AnimatorController animatorController;
    List<ChildAnimatorState> searchedStates = new List<ChildAnimatorState>();
    ChildAnimatorState modelState;
    string searchString = "";
    string modelString = "";
    Vector2 scrollPosition = Vector2.zero; // 滚动视图的位置
    int verticalOffset = 45; // 垂直偏移量

    void OnGUI()
	{
        animatorController = EditorGUILayout.ObjectField("Controller",animatorController, typeof(AnimatorController), true) as AnimatorController;
        if(animatorController == null)
        {
            return;
        }
        if(modelState.state == null && !string.IsNullOrEmpty(modelString))
        {
            modelString = "";
        }
        EditorGUILayout.LabelField("范本节点", modelString);
        searchString = EditorGUILayout.TextField("搜索节点关键词", searchString);
        if (GUILayout.Button("搜索节点"))
        {
            searchedStates.Clear();
            SearchState();
        }
        for(int i=searchedStates.Count-1;i>=0;i--)
        {
            var state = searchedStates[i];
            if(state.state == null)
            {
                searchedStates.RemoveAt(i);
            }
        }
        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.LabelField("搜到结果：");
        if(GUILayout.Button("按名称排序"))
        {
            searchedStates.Sort((x, y) => string.Compare(x.state.name, y.state.name));
        }
        EditorGUILayout.EndHorizontal();
        #if UNITY_2017_1_OR_NEWER
        EditorGUILayout.BeginVertical("TextArea");
        #else
        EditorGUILayout.BeginVertical("AS TextArea");
        #endif
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.MinHeight(80),GUILayout.MaxHeight(800));
        foreach(var state in searchedStates)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("", state.state.name);
            if(GUILayout.Button("删除搜索"))
            {
                searchedStates.Remove(state);
                EditorGUILayout.EndHorizontal();
                break;
            }
            if(GUILayout.Button("记为范本"))
            {
                modelString = state.state.name;
                modelState = state; // 保存当前状态为范本状态
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        verticalOffset = EditorGUILayout.IntSlider("纵向排列偏移量", verticalOffset, 40, 80);
        if(GUILayout.Button("【自动排版】 依照范本节点纵向排列搜到的节点"))
        {
            if(modelState.state != null)
            {
                UpdateState(ref modelState);
                var pos = modelState.position;
                float posY = pos.y;
                modelState.Equals(modelState);
                for(int i=0,len=searchedStates.Count;i<len;i++)
                {
                    var state = searchedStates[i];
                    if(i == 0 && modelState.state == state.state)
                    {
                        posY -= verticalOffset;
                        continue;
                    }
                    pos.y = posY + verticalOffset * (i+1);
                    state.position = pos;
                    EditState(state);
                }
            }
        }
        if(GUILayout.Button("【自动连线】 依照范本节点处理搜到节点的前置连线"))
        {
            if(modelState.state != null)
            {
                UpdateState(ref modelState);
                AnimatorTransitionBase tranPre = FindPreTransition(modelState.state);
                if(tranPre != null)
                {
                    AnimatorStateMachine tranSourceStateMachine = null;
                    AnimatorState stateSource = FindSourceState(tranPre, ref tranSourceStateMachine);
                    int counter = 0;
                    for(int i=0,len=searchedStates.Count; i<len; i++)
                    {
                        var state = searchedStates[i];
                        //跳过范本自身
                        if(modelState.state == state.state)
                        {
                            continue;
                        }
                        counter ++;
                        AnimatorTransitionBase tran = null;
                        if(stateSource != null)
                        {
                            tran = TranEdit(stateSource, state.state, tranPre, counter);
                        }
                        else if(tranSourceStateMachine != null)
                        {
                            if(tranPre is AnimatorStateTransition)
                            {
                                tran = TranEdit_Any(tranSourceStateMachine, state.state, tranPre, counter);
                            }
                            else if(tranPre is AnimatorTransition)
                            {
                                tran = TranEdit_Entry(tranSourceStateMachine, state.state, tranPre, counter);
                            }                            
                        }
                    }
                    EditorUtility.DisplayDialog("连线处理完毕", "界面没有刷新的话, 切换Layer或者其他状态机视图再切回来即可。", "OK");

                }
                else
                {
                    Debug.LogError("找不到范本节点的前置连线");
                }
            }
        }
    }
    #region 编辑连线
    private AnimatorStateTransition TranEdit(AnimatorState baseState, AnimatorState stateForEdit, AnimatorTransitionBase tranPre, int index)
    {
        AnimatorStateTransition result = null;
        foreach(var tran in baseState.transitions)
        {
            if(tran.destinationState == stateForEdit)
            {
                result = tran;
                break;
            }
        }
        if(result == null)
        {
            result = baseState.AddTransition(stateForEdit);
        }
        EditCondition(result, tranPre, index);
        return result;
    }
    private AnimatorStateTransition TranEdit_Any(AnimatorStateMachine baseMachine, AnimatorState stateForEdit, AnimatorTransitionBase tranPre, int index)
    {
        AnimatorStateTransition result = null;
        foreach(var tran in baseMachine.anyStateTransitions)
        {
            if (tran.destinationState == stateForEdit)
            {
                result = tran;
                break;
            }
        }
        if(result == null)
        {
            result = baseMachine.AddAnyStateTransition(stateForEdit);
        }
        EditCondition(result, tranPre, index);
        return result;
    }
    private AnimatorTransition TranEdit_Entry(AnimatorStateMachine baseMachine, AnimatorState stateForEdit, AnimatorTransitionBase tranPre, int index)
    {
        AnimatorTransition result = null;
        foreach (var tran in baseMachine.entryTransitions)
        {
            if (tran.destinationState == stateForEdit)
            {
                result = tran;
                break;
            }
        }
        if(result == null)
        {
            result = baseMachine.AddEntryTransition(stateForEdit);
        }
        EditCondition(result, tranPre, index);
        return result;
    }
    private void EditCondition(AnimatorTransitionBase targetTran, AnimatorTransitionBase modelTran, int index)
    {
        //从modelTran复制条件到targetTran，并在此基础上修改条件值。
        ArrayList arrayList = new ArrayList();
        arrayList.AddRange(modelTran.conditions);
        AnimatorCondition[] targetConditions = arrayList.ToArray(typeof(AnimatorCondition)) as AnimatorCondition[];
        for(int i=0,len=targetConditions.Length;i<len;i++)
        {
            var conditionTarget = targetConditions[i];
            //修改条件值，例如当条件为等于某数值时，让这个数值根据复制次数自增。
            //这里仅为满足作者当前需求，实际应用中可以根据需要继续修改条件逻辑。
            if(conditionTarget.mode == AnimatorConditionMode.Equals)
            {
                conditionTarget.threshold = conditionTarget.threshold + index;
            }
            targetConditions[i] = conditionTarget;
        }
        //------修改完毕，回填数据--------
        targetTran.conditions = targetConditions;

        if(targetTran is AnimatorStateTransition)
        {
            AnimatorStateTransition targetTranA = targetTran as AnimatorStateTransition;
            AnimatorStateTransition modelTranA = modelTran as AnimatorStateTransition;
            targetTranA.duration = modelTranA.duration;
            targetTranA.hasExitTime = modelTranA.hasExitTime;
            targetTranA.hasFixedDuration = modelTranA.hasFixedDuration;
            targetTranA.offset = modelTranA.offset;
            targetTranA.orderedInterruption = modelTranA.orderedInterruption;
        }
    }
    #endregion

    #region 节点遍历
    /// <summary>
    /// 搜索AnimatorController中所有包含关键词的状态节点，并保存到searchedStates列表。
    /// </summary>
    private void SearchState()
    {
        foreach (var layer in animatorController.layers)
        {
            TraverseStateMachineForSearch(layer.stateMachine);
        }
    }
    private void TraverseStateMachineForSearch(AnimatorStateMachine stateMachine)
    {
        if (stateMachine == null)
            return;

        // 遍历状态机中的所有状态
        foreach (var state in stateMachine.states)
        {
            if(state.state.name.Contains(searchString))
            {
                searchedStates.Add(state);
            }
        }
        // 遍历状态机中的所有子状态机
        foreach (var subStateMachine in stateMachine.stateMachines)
        {
            TraverseStateMachineForSearch(subStateMachine.stateMachine); // 递归遍历子状态机
        }
    }
    /// <summary>
    /// 将状态节点写入到目前Animator窗口的数据，并使窗口刷新显示出来。
    /// </summary>
    /// <param name="stateForEdit"></param>
    private void EditState(ChildAnimatorState stateForEdit)
    {
        foreach (var layer in animatorController.layers)
        {
            TraverStateMachineForEdit(layer.stateMachine, stateForEdit);
        }
    }
    private void TraverStateMachineForEdit(AnimatorStateMachine stateMachine, ChildAnimatorState stateForEdit)
    {
        if (stateMachine == null)
            return;

        // 遍历状态机中的所有状态
        for(int i=0,len=stateMachine.states.Length;i< len; i++)
        {
            var state = stateMachine.states[i];
            if(state.state == stateForEdit.state)//找到了
            {
                ArrayList arrayList = new ArrayList();
                arrayList.AddRange(stateMachine.states);
                arrayList[i] = stateForEdit;
                stateMachine.states = arrayList.ToArray(typeof(ChildAnimatorState)) as ChildAnimatorState[];
                return;
            }
        }
        // 遍历状态机中的所有子状态机
        foreach (var subStateMachine in stateMachine.stateMachines)
        {
            TraverStateMachineForEdit(subStateMachine.stateMachine, stateForEdit); // 递归遍历子状态机
        }
    }
    /// <summary>
    /// 使状态节点与目前Animator窗口的最新状态保持同步
    /// </summary>
    /// <param name="stateForUpdate"></param>
    private void UpdateState(ref ChildAnimatorState stateForUpdate)
    {
        foreach (var layer in animatorController.layers)
        {
            TraverStateMachineForUpdate(layer.stateMachine, ref stateForUpdate);
        }
    }
    private void TraverStateMachineForUpdate(AnimatorStateMachine stateMachine, ref ChildAnimatorState stateForUpdate)
    {
        if (stateMachine == null)
            return;

        // 遍历状态机中的所有状态
        for(int i=0,len=stateMachine.states.Length;i< len; i++)
        {
            var state = stateMachine.states[i];
            if(state.state == stateForUpdate.state)//找到了
            {
                stateForUpdate = state;
                return;
            }
        }
        // 遍历状态机中的所有子状态机
        foreach (var subStateMachine in stateMachine.stateMachines)
        {
            TraverStateMachineForUpdate(subStateMachine.stateMachine, ref stateForUpdate); // 递归遍历子状态机
        }
    }
    /// <summary>
    /// 获取连线的起点状态。
    /// </summary>
    /// <param name="transition"></param>
    /// <returns></returns>
    private AnimatorState FindSourceState(AnimatorTransitionBase transition, ref AnimatorStateMachine tranSourceStateMachine)
    {
        if(transition == null) return null;

        AnimatorState state = null;
        foreach (var layer in animatorController.layers)
        {
            state = TraverStateMachineForSource(layer.stateMachine, transition, ref tranSourceStateMachine);

            if(state != null || tranSourceStateMachine != null)
            {
                break;
            }
        }
        return state;
    }
    private AnimatorState TraverStateMachineForSource(AnimatorStateMachine stateMachine, AnimatorTransitionBase transition, ref AnimatorStateMachine tranSourceStateMachine)
    {
        tranSourceStateMachine = null;
        if (transition == null || stateMachine == null)
        {
            Debug.LogError("Transition or StateMachine is null.");
            return null;
        }
        // 遍历状态机中的所有Entry状态过渡
        foreach (var tran in stateMachine.entryTransitions)
        {
            if (tran == transition)
            {
                tranSourceStateMachine = stateMachine;
                return null;
            }
        }
        // 遍历状态机中的所有任意状态过渡
        foreach (var tran in stateMachine.anyStateTransitions)
        {
            if (tran == transition)
            {
                tranSourceStateMachine = stateMachine;
                return null;
            }
        }
        // 遍历状态机中的所有状态
        foreach (var state in stateMachine.states)
        {
            // 遍历状态的过渡
            foreach (var stateTransition in state.state.transitions)
            {
                // 检查过渡是否匹配
                if (stateTransition == transition)
                {
                    return state.state; // 返回源状态
                }
            }
        }
        // 递归遍历子状态机
        foreach (var subStateMachine in stateMachine.stateMachines)
        {
            var sourceState = TraverStateMachineForSource(subStateMachine.stateMachine, transition, ref tranSourceStateMachine);
            if (sourceState != null || tranSourceStateMachine != null)
            {
                return sourceState;
            }
        }

        // 如果没有找到匹配的过渡，返回 null
        return null;
    }
    /// <summary>
    /// 查找节点的前置连线
    /// </summary>
    /// <param name="animatorState"></param>
    /// <returns></returns>
    private AnimatorTransitionBase FindPreTransition(AnimatorState animatorState)
    {
        if(animatorState == null) return null;

        AnimatorTransitionBase tran = null;
        foreach (var layer in animatorController.layers)
        {
            tran = TraverStateMachineForPreTransition(layer.stateMachine, animatorState);
            if(tran != null)
            {
                return tran;
            }
        }
        return null;
    }
    private AnimatorTransitionBase TraverStateMachineForPreTransition(AnimatorStateMachine stateMachine, AnimatorState animatorState)
    {
        if (animatorState == null || stateMachine == null)
        {
            Debug.LogError("AnimatorState or StateMachine is null.");
            return null;
        }
        // 遍历状态机中的所有状态
        foreach (var state in stateMachine.states)
        {
            // 遍历状态的过渡
            foreach (var stateTransition in state.state.transitions)
            {
                // 检查过渡是否匹配
                if (stateTransition.destinationState == animatorState)
                {
                    return stateTransition;
                }
            }
        }
        // 遍历状态机中的所有从AnyState出发的过渡
        foreach (var stateTransition in stateMachine.anyStateTransitions)
        {
            // 检查过渡是否匹配
            if (stateTransition.destinationState == animatorState)
            {
                return stateTransition;
            }
        }
        // 遍历状态机中的所有从Entry出发的过渡
        foreach (var stateTransition in stateMachine.entryTransitions)
        {
            // 检查过渡是否匹配
            if (stateTransition.destinationState == animatorState)
            {
                return stateTransition;
            }
        }

        // 递归遍历子状态机
        foreach (var subStateMachine in stateMachine.stateMachines)
        {
            var tran = TraverStateMachineForPreTransition(subStateMachine.stateMachine, animatorState);
            if (tran != null)
            {
                return tran;
            }
        }

        // 如果没有找到匹配的过渡，返回 null
        return null;
    }

    #endregion
}
#endif