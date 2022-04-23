using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    public Player PlayerPrefab;
    private Player _player;
    private PhotonView _playerView;
    public CinemachineVirtualCamera cinemachineCamera;
    private Vector3 _spawnPosition;
    void Start()
    {
        _spawnPosition = new Vector3(0, 0.2f, 0);
        _player = PhotonNetwork.Instantiate(PlayerPrefab.name, _spawnPosition, Quaternion.identity).GetComponent<Player>();
        _playerView = _player.GetComponent<PhotonView>();
        cinemachineCamera.Follow = _player._cinemachineTarget.transform;
    }

    public void PushButtonOne()
    {
        _playerView.RPC("ChangeTop", RpcTarget.AllBuffered);
    }

    public void PushButtonTwo()
    {
        _playerView.RPC("ChangeMiddle", RpcTarget.AllBuffered);
    }
    public void PushButtonThree()
    {
        _playerView.RPC("ChangeBottom", RpcTarget.AllBuffered);
    }
}
