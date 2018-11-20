using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class VtsController : MonoBehaviour {
	[SerializeField]
	GameObject artworkObj;
	GridView gridView;
	[SerializeField]
	Button nextButton;
	int imgIndex = 0;
	public int ImgId = 10;
	public List<Sprite> images;
	public List<ArtworkData> artworks;
	[SerializeField]
	CommentManager commentManager;
	[SerializeField]
	CommentController commentController;
	ArtworkData art0 = new ArtworkData(180,250,10);
	ArtworkData art1 = new ArtworkData(600,400,9);
	ArtworkData art2 = new ArtworkData(200,300,8);


	// Use this for initialization
	void Start () {
		RectTransform artworkRect = artworkObj.GetComponent<RectTransform>();		
		Image artworkImg = artworkObj.GetComponent<Image>();
		artworks = new List<ArtworkData>(){art0, art1, art2};
		gridView = artworkObj.GetComponentInChildren<GridView>();
		nextButton.OnClickAsObservable().Subscribe(_ => {
			imgIndex = (imgIndex + 1) % 3;
			artworkImg.sprite = images[imgIndex];
			artworkRect.sizeDelta = new Vector2(artworks[imgIndex].width,artworks[imgIndex].height);
			gridView.InitGrid();
			commentManager.workId = artworks[imgIndex].id;
			commentController.UpdateComment();
		});
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
