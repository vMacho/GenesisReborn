using UnityEngine;
using System.Collections;

public class AutoIntensity : MonoBehaviour {

	public Gradient nightDayColor;

	public float maxIntensity = 3f;
	public float minIntensity = 0f;
	public float minPoint = -0.2f;

	public float maxAmbient = 1f;
	public float minAmbient = 0f;
	public float minAmbientPoint = -0.2f;


	public Gradient nightDayFogColor;
	public AnimationCurve fogDensityCurve;
	public float fogScale = 1f;

	public float dayAtmosphereThickness = 0.4f;
	public float nightAtmosphereThickness = 0.87f;

	public Vector3 dayRotateSpeed;
	public Vector3 nightRotateSpeed;

	float skySpeed = 1;


	Light mainLight;
	Skybox sky;
	Material skyMat;

    public Renderer water;
    //public Transform starts;
    public Transform worldProbe;

	void Start () 
	{
	
		mainLight = GetComponent<Light>();
		skyMat = RenderSettings.skybox;

	}

	void Update () 
	{
        //starts.transform.rotation = transform.rotation;

        Vector3 tvec = Camera.main.transform.position;
        worldProbe.transform.position = tvec;

        if (water != null)
        {
            water.material.mainTextureOffset = new Vector2(Time.time / 1000, 0);
            water.material.SetTextureOffset("_DetailAlbedoMap", new Vector2(0, Time.time / 100));
        }

		float tRange = 1 - minPoint;
		float dot = Mathf.Clamp01 ((Vector3.Dot (mainLight.transform.forward, Vector3.down) - minPoint) / tRange);
		float i = ((maxIntensity - minIntensity) * dot) + minIntensity;

		mainLight.intensity = i;

		tRange = 1 - minAmbientPoint;
		dot = Mathf.Clamp01 ((Vector3.Dot (mainLight.transform.forward, Vector3.down) - minAmbientPoint) / tRange);
		i = ((maxAmbient - minAmbient) * dot) + minAmbient;
		RenderSettings.ambientIntensity = i;

		mainLight.color = nightDayColor.Evaluate(dot);
		RenderSettings.ambientLight = mainLight.color;

		RenderSettings.fogColor = nightDayFogColor.Evaluate(dot);
		RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;

		i = ((dayAtmosphereThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
		skyMat.SetFloat ("_AtmosphereThickness", i);
        
        if (dot > 0)
        {
            if (dot < 0.3f)
            {
                if (GameController.current.GetDaylight() == DayPos.amanecer || GameController.current.GetDaylight() == DayPos.noche) GameController.current.SetDaylight(DayPos.amanecer);
                else GameController.current.SetDaylight(DayPos.atardecer);
            }
            else GameController.current.SetDaylight(DayPos.dia);

            transform.Rotate(dayRotateSpeed * Time.deltaTime * skySpeed);
        }
        else
        {
            GameController.current.SetDaylight(DayPos.noche);
            transform.Rotate(nightRotateSpeed * Time.deltaTime * skySpeed);
        }

		/*if (Input.GetKeyDown (KeyCode.Q)) skySpeed *= 0.5f;
		if (Input.GetKeyDown (KeyCode.E)) skySpeed *= 2f;*/


	}
}
