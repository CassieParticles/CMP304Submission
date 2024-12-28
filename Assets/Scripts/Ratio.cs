public class Ratio
{

    public Ratio(int count)
    {
        weights = new float[count];
        sum = 0;
    }
    public Ratio(float[] initialWeights)
    {
        weights = initialWeights;
        sum = 0;
        for (int i = 0; i < weights.Length; ++i)
        {
            sum += weights[i];
        }
    }

    public float[] weights;

    private float sum;

    public float this[int index]
    {
        get=>weights[index];
        set 
        {
            sum-=weights[index];
            weights[index] = value;
            sum += value;
        }
        
    }

    public float GetRatio(int index)
    {
        if(sum==0)
        {
            return 0;
        }
        return weights[index]/sum;
    }

    public float[] GetRatio()
    {
        float[] ratios = new float[weights.Length];
        for(int i=0;i<ratios.Length;i++)
        {
            ratios[i] = GetRatio(i);
        }
        return ratios;
    }
}
