using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes_Library.Domain
{
    public interface IPresenterCallback<T>
    {
        void OnSuccess(T result);
        void OnError(Exception e);
    }

    public interface IUsecaseCallback<T>
    {
        void OnSuccess(T response);
        void OnError(Exception error);
    }           
    public abstract class UseCaseBase<T>
    {
       
        public IPresenterCallback<T> PresenterCallback { get; set; }

        protected UseCaseBase(IPresenterCallback<T> callback) 
        { 
            PresenterCallback= callback;
            
        }

        public void Execute()
        {
            Task.Run(() =>
            {
                if(GetIfCacheAvailable())
                {
                    return;
                }
                try
                {
                    Action();
                }
                catch(Exception e)
                {
                    PresenterCallback?.OnError(e);
                }
            });
        }

        public abstract void Action();
        public bool GetIfCacheAvailable()
        {
            return false;
        }
    }
}
