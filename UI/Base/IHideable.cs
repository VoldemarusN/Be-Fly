using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace UI.LaunchForce
{
    public interface IHideable
    {
        UniTask Show();
        UniTask Hide();
    }
}