using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {
    public TileState State { get; private set; }
    public TileCell Cell { get; private set; }
    public bool Locked { get; set; }
    private Image _background;
    private TextMeshProUGUI _text;

    private void Awake() {
        _background = GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetState(TileState state) {
        State = state;

        _background.color = State.BackgroundColor;
        _text.color = State.TextColor;
        _text.text = State.Number.ToString();
    }

    public void Spawn(TileCell cell) {
        if (Cell != null) {
            Cell.Tile = null;
        }

        Cell = cell;
        Cell.Tile = this;

        transform.position = Cell.transform.position;
    }

    public void MoveTo(TileCell cell) {
        if (Cell != null) {
            Cell.Tile = null;
        }

        Cell = cell;
        Cell.Tile = this;

        StartCoroutine(Animate(Cell.transform.position, false));
    }

    public void Merge(TileCell cell) {
        if (Cell != null) {
            Cell.Tile = null;
        }

        Cell = null;
        cell.Tile.Locked = true;

        StartCoroutine(Animate(cell.transform.position, true));
    }

    private IEnumerator Animate(Vector3 to, bool merging) {
        float elapsed = 0f;
        float duration = 0.1f;

        Vector3 from = transform.position;

        while (elapsed < duration) {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;

        if (merging) {
            Destroy(gameObject);
        }
    }
}
