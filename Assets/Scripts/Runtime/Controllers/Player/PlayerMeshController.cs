using DG.Tweening;
using Runtime.Data.ValueObjects;
using TMPro;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private new Renderer renderer;
        [SerializeField] private TextMeshPro scaleText;
        [SerializeField] private ParticleSystem confetti;

        #endregion

        #region Private Variables

        private PlayerMeshData _data;

        #endregion

        #endregion

        internal void SetData(PlayerMeshData data)
        {
            _data = data;
        }

        internal void ScaleUpPlayer()
        {
            renderer.gameObject.transform.DOScale(_data.ScaleCounter, 0.5f).SetEase(Ease.Flash);
        }

        internal void ShowUpText()
        {
            scaleText.DOFade(1,0).SetEase(Ease.Flash).OnComplete(()=> // animasyon biter bitmez aşağıdakiler çalışacak.
            {
                scaleText.DOFade(0, 0.30f).SetDelay(0.35f); // yukardakini görünmez hale getirdik
                scaleText.rectTransform.DOAnchorPosY(1f, 0.65f).SetEase(Ease.Linear); // 1 birim yukarı 0.65 saniyede
            });
        }

        internal void PlayConfetti()
        {
            confetti.Play();
        }

        internal void OnReset()
        {
            renderer.gameObject.transform.DOScale(1,1).SetEase(Ease.Linear);
        }
    }
}