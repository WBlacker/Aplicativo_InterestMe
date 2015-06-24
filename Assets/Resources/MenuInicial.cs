using UnityEngine;
using System.Collections;
using MySql.Data.MySqlClient;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MenuInicial : MonoBehaviour {

	//Vars de conexao
	MySqlConnection conexao;
	private string stConexao = "Server=localhost;Database=interestme;Uid=root;Pwd=;";
	MySqlCommand comando;

	//Variaveis
	private string nome="";
	private string userName="";
	private string userNameMinuscula="";
	private string email="";
	private string senha="";
	private string idSexo="";
	public bool sexoMasc;
	public bool sexoFem;
	private bool salvar;
	private bool usuarioExistente;
	private bool camposVazios;
	private int animarBotao = 1;
	private int level = 1;
	private int erro = 0;
	public Vector2 scrollPos = Vector2.zero;
	public Texture2D logo;
	public Texture2D textureAnim;
	public GUIStyle campoStyle;
	public GUIStyle labelStyle;
	public GUIStyle boxStyle;
	public GUIStyle botao1Style;
	public GUIStyle botao2Style;
	public GUIStyle checkBox;





	
	void Start () {
	
	}

	IEnumerator FramesCampo() {
		for (int i = 0; i < 25; i++) {
			textureAnim = Resources.Load ("Texture/Sequence_Campo/campo00" + i) as Texture2D;
			campoStyle.normal.background = textureAnim;
			if(i<20){
				textureAnim = Resources.Load ("Texture/Sequence_Botao1/BotaoVerde_" + i) as Texture2D;
				botao1Style.normal.background = textureAnim;
				textureAnim = Resources.Load ("Texture/Sequence_Botao2/BotaoAzul_" + i) as Texture2D;
				botao2Style.normal.background = textureAnim;
			}
			yield return new WaitForSeconds (0.04f);
		}
	}


	void OnGUI(){
		conexao = new MySqlConnection(stConexao);
		comando = new MySqlCommand();
		comando.Connection = conexao;
		
		//conexao.Open();


		GUI.skin.textField = campoStyle;
		GUI.skin.label = labelStyle;
		GUI.skin.box = boxStyle;
		//Login
		if (level == 1) {

			if(animarBotao==1){
				StartCoroutine(FramesCampo());
				animarBotao = 2;
			}
			GUI.DrawTexture (new Rect (Screen.width / 2 - logo.width /3, Screen.height / 2 - (logo.height / 1.5f + 50), logo.width / 1.5f, logo.height / 1.5f), logo);


			GUI.Box (new Rect (Screen.width / 2 - 105, Screen.height / 2 - 40, 210, 190), "LOGIN");

			GUI.Label (new Rect (Screen.width / 2 - 95, Screen.height / 2 - 20, 200, 60), "Usuario:");

			userName = GUI.TextField (new Rect (Screen.width / 2 - 95, Screen.height / 2, 190, 30), userName);

			GUI.Label (new Rect (Screen.width / 2 - 95, Screen.height / 2 + 30, 200, 60), "Senha:");
			senha = GUI.TextField (new Rect (Screen.width / 2 - 95, Screen.height / 2 + 60, 190, 30), senha);

			//Entrar
			if (GUI.Button (new Rect (Screen.width / 2 - 98, Screen.height / 2 + 100, 95, 30),"Entrar", botao1Style)) {
				Login (conexao);
			}
			//Registrar
			if (GUI.Button (new Rect (Screen.width / 2 + 2, Screen.height / 2 + 100, 95, 30), "Registrar", botao2Style)) {
				level = 2;
			}
		}


		//REGISTRAR
		if (level == 2) {
			if(animarBotao==2){
				StartCoroutine(FramesCampo());
				animarBotao = 1;
			}
			GUI.Box (new Rect (Screen.width / 2 - 100, Screen.height/2 - 185, 200, 55), "Nome");
			nome = GUI.TextField (new Rect (Screen.width / 2 - 100 + 5, Screen.height/2 - 165, 190, 30), nome);

			GUI.Box (new Rect (Screen.width / 2 - 100, Screen.height/2 - 125, 200, 55), "Usuario");
			userName = GUI.TextField (new Rect (Screen.width / 2 - 100 + 5, Screen.height/2 - 105, 190, 30), userName);

			GUI.Box (new Rect (Screen.width / 2 - 100, Screen.height/2 - 65, 200, 55), "E-mail");
			email = GUI.TextField (new Rect (Screen.width / 2 - 100 + 5, Screen.height/2 - 45, 190, 30), email);

			GUI.Box (new Rect (Screen.width / 2 - 100, Screen.height/2 - 5, 200, 55), "Senha");
			senha = GUI.TextField (new Rect (Screen.width / 2 - 100 + 5, Screen.height / 2 + 15, 190, 30), senha);

			//RADIO BUTTON PARA SEXO
			GUI.Box (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 55, 200, 60), "Sexo");

			if (sexoMasc = GUI.Toggle(new Rect (Screen.width / 2 - 100 + 10, Screen.height / 2 + 68, 100, 30), sexoMasc, "Masculino",checkBox)) {
				if(sexoMasc== false){
					sexoMasc = true;
				}
				sexoFem = false;
				idSexo = "m";

			}
			if (sexoFem = GUI.Toggle (new Rect (Screen.width / 2 - 100 + 10,Screen.height / 2 + 90, 100, 30), sexoFem, "Feminino",checkBox)) {
				sexoMasc = false;
				idSexo = "f";
	
			}


			//SALVAR CADASTRO
			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 130, 200, 40), "Cadastrar",botao1Style)){
			    if(idSexo != "" && nome != "" && userName != "" && userName != "" && senha != ""){
					userNameMinuscula = userName.ToLower();
					Registrar(conexao);
					if(salvar == true){
						comando.CommandText = "INSERT INTO interestme.usuario(nome, userName,email, senha, idSexo) VALUES ('" + nome + "','" + userNameMinuscula + "','" + email + "','" + senha + "','" + idSexo + "')";
						comando.ExecuteNonQuery ();
						level = 1;
					}
				}
				else{
					camposVazios = true;
				}

			}
			//ERRO CAMPOS VAZIOS
			if(camposVazios){
				GUI.Box (new Rect (Screen.width / 2 - 100, Screen.height/2 - 250, 200, 55),"E necessario preencher\ntodos os campos\npara salvar!");
			}
			//ERRO USUARIO EXISTENTE
			if(usuarioExistente == true){
				GUI.Box (new Rect (Screen.width / 2 + 100 + 5, Screen.height/2 - 115, 100, 40),"Este Usuario \n ja existe.");
			}
			//VOLTAR PARA LOGIN
			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 175, 200, 40), "Voltar",botao2Style)) {
				level = 1;
			}
		}

		Listar(conexao);
		//conexao.Close();
	}


	//LISTAR
	void Listar (MySqlConnection _conexao){
		if (level == 3) {
			int y=0;
			comando = _conexao.CreateCommand ();

			//SELECT
			comando.CommandText = "SELECT * FROM interestme.usuario AS u " +
								 " INNER JOIN interestme.sexo AS s " +
								 " ON s.idSexo=u.idSexo";
			MySqlDataReader dados = comando.ExecuteReader ();
			//BOX BG DA LISTA
			GUI.Box(new Rect(Screen.width / 2 - 105, 20, 210, Screen.height - 50),"Lista de Usuarios");

			//VARIAVEL PARA AUMENTAR SCROLL DINAMICAMENTE(AINDA NAO ESTA FUNCIONANDO)
			int tamScroll = 1000;

			//INICIO SCROLLBAR
			scrollPos = GUI.BeginScrollView(new Rect(Screen.width / 2 - 110, 60, 200, Screen.height - 100), scrollPos, new Rect(Screen.width / 2 - 90, 0, 180,tamScroll));
			//LOOP PARA CRIACAO DA LISTA
			while (dados.Read()) {
				string nome = (string)dados ["nome"];
				string userName = (string)dados ["userName"];
				string email = (string)dados ["email"];
				string senha = (string)dados ["senha"];
				string sexo = (string)dados ["descricao"];

				GUI.Box (new Rect (Screen.width / 2 - 70, 0+y*175, 150, 170), 
				         "Nome:" + nome + 
				         "\n\n Usuario: " + userName +
				         "\n E-mail: " + email +
				         "\n Senha: " + senha +
				         "\n Sexo: " + sexo);
				GUI.Button (new Rect (Screen.width / 2 - 65, 100+y*175, 140, 30), "Interessante",botao1Style);
				GUI.Button (new Rect (Screen.width / 2 - 65, 135+y*175, 140, 30), "Desinteressante",botao1Style);
				y++;
			}
			dados.Close();
			//INICIO SCROLLBAR
			GUI.EndScrollView();
		}

	}
	
	//LOGIN
	void Login (MySqlConnection _conexao){
		comando = _conexao.CreateCommand ();
		//SELECT
		comando.CommandText = "SELECT * FROM interestme.usuario";
		MySqlDataReader dados = comando.ExecuteReader ();
		while (dados.Read()) {

			string CnxUserName = (string)dados ["userName"];
			string CnxSenha = (string)dados ["senha"];
			if(userName == CnxUserName && senha == CnxSenha ){
				level = 3;
				erro = 0;
				break;
			}
			else{
				erro = 1;
				break;
			}
		}
		dados.Close();
		if (erro == 1) {
			Debug.Log("Usuario e Senha incorretos.");		
		}
		
	}

	//REGISTRAR
	void Registrar(MySqlConnection _conexao){
		comando = _conexao.CreateCommand ();
		//SELECT
		comando.CommandText = "SELECT * FROM interestme.usuario";
		MySqlDataReader dados = comando.ExecuteReader ();
		while (dados.Read()) {
			string CnxUserName = (string)dados ["userName"];
			if(userNameMinuscula != CnxUserName){
				salvar = true;

			}else{
				usuarioExistente = true;
				salvar = false;
				break;
			}
		}
		dados.Close();
		}
		
}
