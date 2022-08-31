using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Image _imageHealth;
    [SerializeField] private TextMeshProUGUI _textHealth;
    [SerializeField] private Canvas _canvasForHealth;

    private float _offsetAngleForCanvas = 180;
    private Enemy _enemy;

    public void Initialize(Enemy enemy)
    {
        _enemy = enemy;
        _textHealth.text = _enemy.Health.ToString();
        ActivateUI();
        RotationCanvas();
    }

    private void RotationCanvas()
    {
        _canvasForHealth.transform.localRotation = Quaternion.Euler(0, (_offsetAngleForCanvas - transform.rotation.eulerAngles.y + _offsetAngleForCanvas), 0);
    }

    public void ChangeHealth()
    {
        _imageHealth.fillAmount = _enemy.Health / _enemy.MaxHealth;
        _textHealth.text = _enemy.Health.ToString();
    }

    public void ActivateUI()
    {
        _canvasForHealth.gameObject.SetActive(true);
    }

    public void DisableUI()
    {
        _canvasForHealth.gameObject.SetActive(false);
    }
}
