using UnityEngine;

public static class AudioClipExt {
	public static void PlayOneShot ( this AudioClip audioClip, AudioSource audioSource, float volumeScale = 1 ) {
		if ( audioClip == null || audioSource == null )
			return;

		audioSource.PlayOneShot ( audioClip, volumeScale );
	}
}
