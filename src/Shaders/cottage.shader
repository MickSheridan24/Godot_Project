shader_type canvas_item;
void fragment(){
   vec2 st = gl_FragCoord.xy/u_resolution.xy;
    vec3 color = vec3(0.0);

    // bottom-left
    vec2 bl = step(vec2(0.1),st);
    vec2 tr = step(vec2(0.1),1.0-st);   // top-right
	color = vec3(bl.x * bl.y * tr.x * tr.y);
    
    float pct = bl.x * bl.y;

    // top-right
    // vec2 tr = step(vec2(0.1),1.0-st);
    // pct *= tr.x * tr.y;



    gl_FragColor = vec4(color,0.5);
}