<?php

namespace App\Http\Controllers;
use Illuminate\Http\Request;
use App\Models\User;
use Firebase\JWT\JWT;
use Laravel\Lumen\Routing\Controller as BaseController;
class AuthController extends Controller
{
    private $request;

    public function __construct(Request $request)
    {
        $this->request = $request;
    }

    public function jwt(User $user){
        $payload =[
            'iss' => "api-rest-lumen-jwt", // Issuer of the token
            'sub' => $user->id, // Subject of the token
            'iat' => time(), // Time when JWT was issued.
            'exp' => time() + 60*60 // Expiration time
        ];
        return JWT::encode($payload, env('JWT_SECRET'),'HS256');
    }

    public function authenticate(User $user){
        $this->validate($this->request, [
            'email' => 'required|email',
            'password' => 'required'
        ]);
        $user = User::where('email', $this->request->input('email'))->first();
        if(!$user){
            return response()->json([
                'error' => 'Email does not exist.'
            ], 400);
        }
        if($this->request->input('password') == $user->password){
            return response()->json([
                'token' => $this->jwt($user)
            ], 200);
        }
        return response()->json([
            'error' => 'Email or password is wrong.'
        ], 400);
    }
}
