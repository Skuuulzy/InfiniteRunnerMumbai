public readonly struct BufferedInput
{
    private readonly int _framePressed;
    private readonly int _duration;

    public BufferedInput(int framePressed, int duration)
    {
        _framePressed = framePressed;
        _duration = duration;
    }

    public bool IsAlive(int currentFrame)
    {
        return currentFrame - _framePressed <= _duration;
    }
}