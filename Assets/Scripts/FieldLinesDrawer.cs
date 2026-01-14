using UnityEngine;

public class FieldLinesDrawer : MonoBehaviour
{
    [Header("Impostazioni Linee")]
    public Color lineColor = Color.white;
    public float lineWidth = 0.1f;
    public Material lineMaterial;
    
    [Header("Dimensioni Campo")]
    public float fieldWidth = 10f;
    public float fieldHeight = 6f;
    
    void Start()
    {
        DrawAllCircles();
    }

        void DrawAllCircles()
    {
        // Cerchio centrale
        DrawCircle("CerchioCentrale", Vector3.zero, 1f, 50);
        
        // Punto centrale (cerchio piccolo)
        DrawCircle("PuntoCentrale", Vector3.zero, 0.1f, 20);
        
        // Arco area rigore sinistra
        Vector3 leftPenaltySpot = new Vector3(-fieldWidth/2 + 1.5f, 0, 0);
        DrawArc("ArcoSinistra", leftPenaltySpot, 1f, -53f, 53f, 30);
        
        // Arco area rigore destra
        Vector3 rightPenaltySpot = new Vector3(fieldWidth/2 - 1.5f, 0, 0);
        DrawArc("ArcoDestra", rightPenaltySpot, 1f, 127f, 233f, 30);
        
        // Dischetto rigore sinistro
        DrawCircle("DischettoSinistro", leftPenaltySpot, 0.08f, 20);
        
        // Dischetto rigore destro
        DrawCircle("DischettoDestro", rightPenaltySpot, 0.08f, 20);
        
        // Cerchi d'angolo (opzionali)
        DrawCornerCircles();
    }
    
    void DrawCircle(string name, Vector3 center, float radius, int segments)
    {
        GameObject circleObj = new GameObject(name);
        circleObj.transform.parent = transform;
        
        LineRenderer line = circleObj.AddComponent<LineRenderer>();
        SetupLineRenderer(line, segments + 1);
        
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = center.x + Mathf.Cos(angle) * radius;
            float y = center.y + Mathf.Sin(angle) * radius;
            line.SetPosition(i, new Vector3(x, y, 0));
            angle += 2f * Mathf.PI / segments;
        }
    }
    
    void DrawArc(string name, Vector3 center, float radius, float startAngle, float endAngle, int segments)
    {
        GameObject arcObj = new GameObject(name);
        arcObj.transform.parent = transform;
        
        LineRenderer line = arcObj.AddComponent<LineRenderer>();
        SetupLineRenderer(line, segments + 1);
        
        float angleStep = (endAngle - startAngle) / segments;
        
        for (int i = 0; i <= segments; i++)
        {
            float angle = startAngle + (angleStep * i);
            float x = center.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float y = center.y + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            line.SetPosition(i, new Vector3(x, y, 0));
        }
    }
    
    void DrawCornerCircles()
    {
        float cornerRadius = 0.3f;
        float offsetX = fieldWidth / 2;
        float offsetY = fieldHeight / 2;
        
        // Angolo in alto a sinistra
        DrawArc("AngoloAltoSx", new Vector3(-offsetX, offsetY, 0), cornerRadius, 0f, 90f, 15);
        
        // Angolo in alto a destra
        DrawArc("AngoloAltoDx", new Vector3(offsetX, offsetY, 0), cornerRadius, 90f, 180f, 15);
        
        // Angolo in basso a sinistra
        DrawArc("AngoloBassoSx", new Vector3(-offsetX, -offsetY, 0), cornerRadius, 270f, 360f, 15);
        
        // Angolo in basso a destra
        DrawArc("AngolobassoDx", new Vector3(offsetX, -offsetY, 0), cornerRadius, 180f, 270f, 15);
    }
    
    void SetupLineRenderer(LineRenderer line, int pointCount)
    {
        line.positionCount = pointCount;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.startColor = lineColor;
        line.endColor = lineColor;
        line.useWorldSpace = false;
        line.loop = false;
        
        // Imposta il materiale se disponibile
        if (lineMaterial != null)
        {
            line.material = lineMaterial;
        }
        else
        {
            // Usa materiale default
            line.material = new Material(Shader.Find("Sprites/Default"));
        }
        
        // Impostazioni per rendering 2D
        line.sortingOrder = 1;
    }
}


