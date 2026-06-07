using UnityEngine;

[CreateAssetMenu(fileName = "AnimationAsset", menuName = "Scriptable Objects/AnimationAsset")]
public class AnimationAsset : ScriptableObject
{
    [SerializeField]
    private Sprite[] frames;

    [SerializeField]
    private float framesPerSecond = 12f;

    public Sprite[] Frames => frames;
    public float FramesPerSecond => framesPerSecond;

    public float FrameDuration => framesPerSecond > 0f ? 1f / framesPerSecond : 0f;
}
