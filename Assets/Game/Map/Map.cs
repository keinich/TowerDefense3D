using UnityEngine;

public class Map : MonoBehaviour {

  public enum ExitPosition {
    None, Bottom, Top, Left, Right
  }

  public Transform nodeParent;
  public Transform generatedMapParent;

  public int width;
  public int height;

  public GameObject nodePrefab;

  public GameObject groundTilePrefab;
  public GameObject pathTilePrefab;

  private float tileHeight => groundTilePrefab ? groundTilePrefab.GetComponent<Renderer>().bounds.size.z : 0;
  private float tileWidth => groundTilePrefab ? groundTilePrefab.GetComponent<Renderer>().bounds.size.x : 0;

  private float mapTileWidth => tileWidth * width;
  private float mapTileHeight => tileHeight * height;

  //private GameObject[,] mapTiles;

  // Start is called before the first frame update
  void Start() {
  }

  // Update is called once per frame
  void Update() {

  }

  public void Generate() {
    //mapTiles = new GameObject[width, height];
    generatedMapParent.gameObject.DestroyAllChildren();
    nodeParent.gameObject.DestroyAllChildren();

    //for (int i = generatedMapParent.transform.childCount; i > 0; --i)
    //  DestroyImmediate(generatedMapParent.transform.GetChild(0).gameObject);
    //for (int i = nodeParent.transform.childCount; i > 0; --i)
    //  DestroyImmediate(generatedMapParent.transform.GetChild(0).gameObject);

    Vector3 position = this.gameObject.transform.position;
    position = Generate(position, ExitPosition.Bottom, ExitPosition.Top);
    position = Generate(position, ExitPosition.Bottom, ExitPosition.Right);
    position = Generate(position, ExitPosition.Left, ExitPosition.Right);
    position = Generate(position, ExitPosition.Left, ExitPosition.Top);
    position = Generate(position, ExitPosition.Bottom, ExitPosition.Top);

    Generate(this.gameObject.transform.position + new Vector3(mapTileWidth, 0, 0), ExitPosition.None, ExitPosition.None);
    Generate(this.gameObject.transform.position + new Vector3(2 * mapTileWidth, 0, 0), ExitPosition.None, ExitPosition.None);

    Generate(this.gameObject.transform.position + new Vector3(0, 0, 2 * mapTileHeight), ExitPosition.None, ExitPosition.None);
    Generate(this.gameObject.transform.position + new Vector3(mapTileWidth, 0, 2 * mapTileHeight), ExitPosition.None, ExitPosition.None);

    Generate(this.gameObject.transform.position + new Vector3(- mapTileWidth, 0, 0 * mapTileHeight), ExitPosition.None, ExitPosition.None);
    Generate(this.gameObject.transform.position + new Vector3(- mapTileWidth, 0, 1 * mapTileHeight), ExitPosition.None, ExitPosition.None);
    Generate(this.gameObject.transform.position + new Vector3(- mapTileWidth, 0, 2 * mapTileHeight), ExitPosition.None, ExitPosition.None);

    Generate(this.gameObject.transform.position + new Vector3(3 * mapTileWidth, 0, 0 * mapTileHeight), ExitPosition.None, ExitPosition.None);
    Generate(this.gameObject.transform.position + new Vector3(3 * mapTileWidth, 0, 1 * mapTileHeight), ExitPosition.None, ExitPosition.None);
    Generate(this.gameObject.transform.position + new Vector3(3 *  mapTileWidth, 0, 2 * mapTileHeight), ExitPosition.None, ExitPosition.None);



  }

  public Vector3 Generate(Vector3 offset, ExitPosition entry, ExitPosition exit) {

    for (int i = 0; i < width; i++) {
      for (int j = 0; j < height; j++) {
        if (IsPath(i, j, entry, exit)) {
          Instantiate(
           pathTilePrefab,
           new Vector3(tileWidth * i, 0, tileWidth * j) + offset, Quaternion.Euler(-90, 0, 0),
           generatedMapParent
         );
          Instantiate(
           nodePrefab,
           new Vector3(tileWidth * i, 0, tileWidth * j) + offset, Quaternion.Euler(-90, 0, 0),
           nodeParent
         );
        } else {
          Instantiate(
            groundTilePrefab,
            new Vector3(tileHeight * i, 0, tileHeight * j) + offset, Quaternion.Euler(-90, 0, 0),
            generatedMapParent
          );
        }
      }
    }

    Vector3 result = new Vector3(0, 0, 0);
    switch (exit) {
      case ExitPosition.None:
        break;
      case ExitPosition.Bottom:
        result = offset + new Vector3(0, 0, -mapTileHeight);
        break;
      case ExitPosition.Top:
        result = offset + new Vector3(0, 0, +mapTileHeight);
        break;
      case ExitPosition.Left:
        result = offset + new Vector3(-mapTileWidth, 0, 0);
        break;
      case ExitPosition.Right:
        result = offset + new Vector3(mapTileWidth, 0, 0);
        break;
    }

    return result;
  }

  private bool IsPath(int i, int j, ExitPosition entry, ExitPosition exit) {
    int halfWidth = Mathf.FloorToInt(width / 2);
    int halfHeight = Mathf.FloorToInt(height / 2);

    switch (entry) {
      case ExitPosition.Bottom:
        if (i == halfWidth && j <= halfHeight) {
          return true;
        }
        break;
      case ExitPosition.Top:
        if (i == halfWidth && j >= halfHeight) {
          return true;
        }
        break;
      case ExitPosition.Left:
        if (j == halfHeight && i <= halfWidth) {
          return true;
        }
        break;
      case ExitPosition.Right:
        if (j == halfHeight && i >= halfWidth) {
          return true;
        }
        break;
    }

    switch (exit) {
      case ExitPosition.Bottom:
        if (i == halfWidth && j <= halfHeight) {
          return true;
        }
        break;
      case ExitPosition.Top:
        if (i == halfWidth && j >= halfHeight) {
          return true;
        }
        break;
      case ExitPosition.Left:
        if (j == halfHeight && i <= halfWidth) {
          return true;
        }
        break;
      case ExitPosition.Right:
        if (j == halfHeight && i >= halfWidth) {
          return true;
        }
        break;
    }
    return false;
  }

}
