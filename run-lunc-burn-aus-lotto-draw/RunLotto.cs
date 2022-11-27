namespace run_lunc_burn_aus_lotto_draw;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void RunLotto()
    {
        LuncBurnAusLottoDraw.Main();
        Assert.Pass();
    }
}
