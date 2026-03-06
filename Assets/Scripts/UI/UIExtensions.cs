#nullable enable
using UnityEngine;

namespace HackedDesign.UI
{
    public static class UIExtensions
    {
        public static void HideIfValid<T>(this T? presenter, Object context, string contextName) where T : AbstractPresenter
        {
            if (presenter.EnsureNotNull(context, contextName))
            {
                presenter.Hide();
            }
        }
    }
}