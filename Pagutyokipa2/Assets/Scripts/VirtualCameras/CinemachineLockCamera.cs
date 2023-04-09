using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Cinemachine
{
    [ExecuteInEditMode]
    [AddComponentMenu("")]
    public class CinemachineLockCamera : CinemachineExtension
    {
        public LockAccess lockX;
        public LockAccess lockY;
        public LockAccess lockZ;

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                Vector3 position = state.RawPosition;
                if (lockX.isLocked) position.x = lockX.GetValue(position.x);
                if (lockY.isLocked) position.y = lockY.GetValue(position.y);
                if (lockZ.isLocked) position.z = lockZ.GetValue(position.z);
                state.RawPosition = position;
            }
        }
    }

    [Serializable]
    public class LockAccess
    {
        public bool isLocked;
        public bool range;
        public float value;
        public float minValue;
        public float maxValue;

        public float GetValue(float nowValue)
        {
            if (!range) return value;

            return Mathf.Clamp(nowValue, minValue, maxValue);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CinemachineLockCamera))]
    public class CinemachineLockCameraEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            CinemachineLockCamera cinemachineLockCamera = (CinemachineLockCamera)target;

            ShowLockAcess(cinemachineLockCamera.lockX, "LockX");
            ShowLockAcess(cinemachineLockCamera.lockY, "LockY");
            ShowLockAcess(cinemachineLockCamera.lockZ, "LockZ");

            void ShowLockAcess(LockAccess lockAccess, string name)
            {
                lockAccess.isLocked = EditorGUILayout.Toggle(name, lockAccess.isLocked);

                if (!lockAccess.isLocked) return;

                EditorGUI.indentLevel++;

                lockAccess.range = EditorGUILayout.Toggle("range", lockAccess.range);

                if (!lockAccess.range)
                {
                    lockAccess.value = EditorGUILayout.FloatField("Value", lockAccess.value);
                }
                else
                {
                    EditorGUI.indentLevel++;
                    lockAccess.minValue = EditorGUILayout.FloatField("MinValue", lockAccess.minValue);
                    lockAccess.maxValue = EditorGUILayout.FloatField("MaxValue", lockAccess.maxValue);

                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
        }
    }
#endif
}
