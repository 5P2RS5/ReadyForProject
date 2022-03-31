public class SingletonClass
{
    private SingletonClass(){ }
    
    static SingletonClass instance;

    public static SingletonClass Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SingletonClass();
            }
            return instance;
        }
    }
}