﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class selectElder : MonoBehaviour {

	[SerializeField] SkinnedMeshRenderer glow;
	[SerializeField] GameObject _img;
	[SerializeField] fadeOutFadein _fd;
	[SerializeField] Animator[] anim;

	void Start()
	{
		foreach(Animator a in anim)
			a.SetBool("talk", false);

		controller.dungeonLevel = 1;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.F4))
		{
			SceneManager.LoadScene("mainMenu");
		}
	}

	void OnMouseEnter()
	{
		glow.enabled = true;
		_img.gameObject.SetActive(true);
		foreach(Animator a in anim)
			a.SetBool("talk", true);
	}
	
	void OnMouseExit()
	{
		foreach(Animator a in anim)
			a.SetBool("talk", false);
		glow.enabled = false;
		_img.gameObject.SetActive(false);
	}

	void OnMouseUpAsButton()
	{
		foreach(Animator a in anim)
			a.SetBool("talk", true);
		_fd.enabled = true;
		enabled = false;
	}




}
